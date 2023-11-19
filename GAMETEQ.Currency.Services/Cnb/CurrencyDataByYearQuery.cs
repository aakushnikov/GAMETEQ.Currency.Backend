using GAMETEQ.Currency.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAMETEQ.Currency.Services.Cnb;

public sealed class CurrencyDataByYearQuery : HttpRequestBase
{
    [FromQuery(Name = "year")]
    // It's get used through reflection in the base class
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Year { get; set; }
}