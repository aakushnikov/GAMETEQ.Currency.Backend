namespace GAMETEQ.Currency.Model
{
    public record CurrencyTick (string Currency, int LotSize, DateTime UpdateDate, decimal Value);
}