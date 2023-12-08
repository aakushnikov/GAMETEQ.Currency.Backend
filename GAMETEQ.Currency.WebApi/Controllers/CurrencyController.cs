using GAMETEQ.Currency.Extensions;
using GAMETEQ.Currency.Model;
using GAMETEQ.Currency.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GAMETEQ.Currency.WebApi.Controllers;

[ApiController]
[Route("currency")]
public sealed class CurrencyController(ISender mediator) : ControllerBase
{
    [HttpGet("list")]
    [SwaggerOperation("Gets the list of all currencies")]
    public async Task<ActionResult<IEnumerable<string>>> GetCurrenciesList()
    {
        var request = new GetCurrenciesListRequest();
        
        var result = await mediator.Send(request);
        
        return Ok(result);
    }
    
    [HttpGet("{currency}/{dateTime}")]
    [SwaggerOperation($"Gets the currency value on specified date: {DateTimeHelpers.DefaultDateTimeFormat}")]
    public async Task<ActionResult<CurrencyTick>> GetValue(string currency, string dateTime)
    {
        var request = new GetCurrencyRequest
        {
            Currency = currency.ToUpper(),
            Date = dateTime,
        };

        var result = await mediator.Send(request);

        return Ok(result);
    }
}