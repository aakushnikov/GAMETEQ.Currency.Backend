using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GAMETEQ.Currency.Data;
using GAMETEQ.Currency.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GAMETEQ.Currency.Services.Cnb;

public sealed class CnbCurrenciesParser : IDisposable
{
    private record ParsedColumnInfo(string Name, int LotSize);
    
    private readonly HttpClient _httpClient = new();
    private readonly ILogger _logger;
    private readonly string _sourceBaseUrl;
    private readonly ICurrencyDbContext _currencyDbContext;

    public CnbCurrenciesParser(string sourceBaseUrl, ICurrencyDbContext currencyDbContext, ILogger logger) =>
        (_sourceBaseUrl, _currencyDbContext, _logger) = (sourceBaseUrl, currencyDbContext, logger);

    public async Task<int> ExtractAndImportData(CurrencyDataByYearQuery request)
    {
        var query = request.ToQuery();
        var baseUri = new Uri(_sourceBaseUrl);
        var url = new Uri(baseUri, query);
        
        var response = await _httpClient.GetAsync(url);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get currencies data by URL: '{url}'", url);
            throw;
        }
        
        var data = await response.Content.ReadAsStringAsync();

        var ticks = Extract(data);
        return await Import(ticks);
    }

    private  static IEnumerable<CurrencyTick> Extract(string data)
    {
        const char rowDataDelimiter = '|';
        const char columnInfoDelimiter = ' ';
        const string dateTimeFormat = "dd.MM.yyyy";
        
        
        var rowsTemp = data.Split(new [] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        switch (rowsTemp.Length)
        {
            case 0:
                throw new DataException("No data for import");
            case 1:
                throw new DataException("There is only first row in data for import which expected as columns description");
        }

        
        var rawDataTemp = new ConcurrentDictionary<int, string[]>();

        Parallel.For(0, rowsTemp.Length, i =>
        {
            var items = rowsTemp[i].Split(rowDataDelimiter);
            rawDataTemp.TryAdd(i, items);
        });

        var rawData = rawDataTemp
            .Where(item => item.Value.Length > 1)
            .ToDictionary(key => key.Key, value => value.Value);
        

        var cols = rawData[0]
            .Skip(1)
            .Select((item, columnIndex) => (
                Index: columnIndex,
                ColumnRaw: item.Split(columnInfoDelimiter))
            )
            .Select(item => (
                    item.Index,
                    item.ColumnRaw.Length == 2
                        ? item.ColumnRaw[1]
                        : throw new FormatException(
                            $"Column has a bad format: '{string.Join(columnInfoDelimiter, item.ColumnRaw)}.' " +
                            $"Expected lot size and currency name together and space-separated."),
                    int.TryParse(item.ColumnRaw[0], NumberStyles.Integer, null, out var lotSize)
                        ? lotSize
                        : throw new FormatException(
                            $"Column has a bad format: '{string.Join(columnInfoDelimiter, item.ColumnRaw)}'. " +
                            $"Lot size should be an {nameof(Int32)}")
                )
            )
            .ToDictionary(
                key => key.Item1,
                value => new ParsedColumnInfo(value.Item2, value.Item3)
            );
        

        var result = rawData
            .Skip(1)
            .Select((rowData, rowIndex) => (
                    RowIndex: rowIndex,
                    Date: DateTime.TryParseExact(rowData.Value[0], dateTimeFormat, null, DateTimeStyles.None,
                        out var date)
                        ? date
                        : throw new FormatException(
                            $"Bad format in row {rowIndex}. Value '{rawData[0]}' is not a {nameof(DateTime)}"),
                    TickData: rowData.Value.Skip(1).Select((item, index) => (
                            RowIndex: rowIndex,
                            ColumnIndex: index,
                            CurrencyInfo: new CurrencyTick(
                                cols[index].Name,
                                cols[index].LotSize,
                                date,
                                decimal.TryParse(item, NumberStyles.Any, new CultureInfo("en-US"), out var value)
                                    ? value
                                    : throw new FormatException(
                                        $"Nad format in row {rowIndex}, col {index}. Value {item} is not a {nameof(Decimal)}")
                            )
                        )
                    )
                )
            )
            .SelectMany(item => item.TickData)
            .Select(item => item.CurrencyInfo)
            .ToList();

        _ = result
            .GroupBy(item => (item.UpdateDate, item.Currency))
            .Any(group => group.Count() == 1
                ? false
                : throw new ConstraintException(
                    $"{group.Key.UpdateDate.ToString(dateTimeFormat)} and {group.Key.Currency} meets twice"));
        
        return result;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0005: Database issues")]
    private async Task<int> Import(IEnumerable<CurrencyTick> ticks)
    {
        var data = ticks.ToArray();
        var updates = data
            .Join(_currencyDbContext.CurrenciesTicks,
                source => new { source.UpdateDate, source.Currency },
                destination => new { destination.UpdateDate, destination.Currency },
                (source, _) => source)
            .ToArray();

        var adds = data.Except(updates).ToArray();
        
        await _currencyDbContext.CurrenciesTicks.AddRangeAsync(adds);
        _currencyDbContext.CurrenciesTicks.UpdateRange(updates);
        return await _currencyDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}