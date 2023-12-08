using MediatR;

namespace GAMETEQ.Currency.WebApi.Requests;

public sealed class GetCurrenciesListRequest : IRequest<IEnumerable<string>>
{
}