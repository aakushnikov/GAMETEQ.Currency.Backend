using GAMETEQ.Currency.Model;
using MediatR;

namespace GAMETEQ.Currency.WebApi.Requests;

public sealed class GetCurrencyRequest : IRequest<CurrencyTick>
{
    public string Currency { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
}