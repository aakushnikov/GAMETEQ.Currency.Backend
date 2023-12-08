using GAMETEQ.Currency.Data;
using GAMETEQ.Currency.WebApi.Requests;
using MediatR;

namespace GAMETEQ.Currency.WebApi.Handlers;

public sealed class GetCurrenciesListHandler(ICurrencyDbContext currencyDbContext) 
    : IRequestHandler<GetCurrenciesListRequest, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetCurrenciesListRequest request, CancellationToken cancellationToken)
    {
        var result = currencyDbContext.CurrenciesTicks
            .Select(item => item.Currency)
            .Distinct()
            .AsEnumerable();
        return Task.FromResult(result);
    }
}