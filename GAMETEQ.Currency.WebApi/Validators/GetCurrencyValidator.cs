using FluentValidation;
using GAMETEQ.Currency.Extensions;
using GAMETEQ.Currency.WebApi.Requests;

namespace GAMETEQ.Currency.WebApi.Validators;

public sealed class GetCurrencyValidator : AbstractValidator<GetCurrencyRequest>
{
    public GetCurrencyValidator()
    {
        RuleFor(request => request.Currency)
            .Must(currency => !string.IsNullOrWhiteSpace(currency))
            .WithMessage($"{nameof(GetCurrencyRequest.Currency)} cannot be null, empty or white-spaced");
        
        RuleFor(request => request.Date)
            .Must(date => !string.IsNullOrWhiteSpace(date))
            .WithMessage($"{nameof(GetCurrencyRequest.Date)} cannot be null, empty or white-spaced")
            .Must(date => date.TryGetDate(out _))
            .WithMessage($"{nameof(GetCurrencyRequest.Date)} is not a valid {nameof(DateTime)} format");
    }
}