using GAMETEQ.Currency.Configuration;
using GAMETEQ.Currency.Data;
using GAMETEQ.Currency.Services.Cnb;
using Microsoft.Extensions.Logging;

namespace GAMETEQ.Currency.Import;

public sealed class Processor
{
    private readonly ISettings _settings;
    private readonly ILogger _logger;
    private readonly ICurrencyDbContext _currencyDbContext;

    public Processor(ISettings settings, ILogger logger, ICurrencyDbContext currencyDbContext) =>
        (_settings, _logger, _currencyDbContext) = (settings, logger, currencyDbContext);
    
    public async Task<int> Run(string[] args)
    {
        Console.WriteLine("This application imports currency data from the source for current year.");
        Console.WriteLine("Any previous years can be specified by passing the year as an argument to the command line");
        Console.WriteLine("Application returns:");
        Console.WriteLine(Exit.GetCodesDescription());

        var year = DateTime.Now.Year;

        switch (args.Length)
        {
            case 0:
                break;

            case 1:
                if (!int.TryParse(args.First(), out year))
                    return Exit.WithCode(ExitCode.ArgNotInt);

                if (year > DateTime.Now.Year)
                    return Exit.WithCode(ExitCode.FutureYear);

                break;

            default:
                return Exit.WithCode(ExitCode.TooManyArgs);
        }

        try
        {
            await Import(year);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return Exit.WithCode(ExitCode.ImportFailed);
        }

        return Exit.WithCode(ExitCode.Success);
    }

    private async Task Import(int year)
    {
        var parser = new CnbCurrenciesParser(_settings.CurrencySourceUrl, _currencyDbContext, _logger);
        
        var request = new CurrencyDataByYearQuery
        {
            Year = year
        };
        
        _logger.LogInformation("Starting import for {year} year from '{_settings.CurrencySourceUrl}'",
            year, _settings.CurrencySourceUrl);

        await parser.ExtractAndImportData(request);
    }
}