using FluentValidation;
using GAMETEQ.Currency.Data;
using GAMETEQ.Currency.Exceptions;
using GAMETEQ.Currency.Extensions;
using GAMETEQ.Currency.Model;
using GAMETEQ.Currency.WebApi.Requests;
using GAMETEQ.Currency.WebApi.Validators;
using MediatR;

namespace GAMETEQ.Currency.WebApi.Handlers;

public sealed class GetCurrencyHandler(ICurrencyDbContext currencyDbContext)
    : IRequestHandler<GetCurrencyRequest, CurrencyTick>
{
    private readonly GetCurrencyValidator _validator = new ();
    
    public async Task<CurrencyTick> Handle(GetCurrencyRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        _ = request.Date.TryGetDate(out var date);

        var result = currencyDbContext.CurrenciesTicks
                         .Where(c => c.Currency == request.Currency && c.UpdateDate <= date)
                         .OrderByDescending(c => c.UpdateDate)
                         .FirstOrDefault()
                     ?? throw new EntityNotFoundException<CurrencyTick>();

        return result;
    }
}