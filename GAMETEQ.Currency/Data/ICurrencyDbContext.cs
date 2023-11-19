using GAMETEQ.Currency.Model;
using Microsoft.EntityFrameworkCore;

namespace GAMETEQ.Currency.Data;

public interface ICurrencyDbContext
{ 
    DbSet<CurrencyTick> CurrenciesTicks { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}