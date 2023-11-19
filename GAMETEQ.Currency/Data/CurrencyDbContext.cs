using System.Data;
using GAMETEQ.Currency.Data.EntityTypeConfigurations;
using GAMETEQ.Currency.Model;
using Microsoft.EntityFrameworkCore;

namespace GAMETEQ.Currency.Data;

public sealed class CurrencyDbContext : DbContext, ICurrencyDbContext
{
    public DbSet<CurrencyTick> CurrenciesTicks { get; set; } = null!;
    
    public CurrencyDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CurrenciesTicksConfiguration());
    }

//     public async Task ImportCurrenciesTicks(IEnumerable<CurrencyTick> ticks)
//     {
//         var transaction = await Database.BeginTransactionAsync(IsolationLevel.Serializable);
//             
//         var tableName = $"temp-{nameof(CurrenciesTicks)}-{Guid.NewGuid()}";
//
//         await transaction.CommitAsync();
//
//     }

}