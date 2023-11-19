using GAMETEQ.Currency.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GAMETEQ.Currency.Data.EntityTypeConfigurations;

public class CurrenciesTicksConfiguration : IEntityTypeConfiguration<CurrencyTick>
{
    public void Configure(EntityTypeBuilder<CurrencyTick> builder)
    {
        builder.HasKey(game => new { game.UpdateDate, game.Currency });
        
        builder.HasIndex(game => new { game.UpdateDate, game.Currency }).IsUnique();
        
        builder.Property(game => game.Currency).IsRequired().HasMaxLength(3);
        builder.Property(game => game.UpdateDate).IsRequired().HasConversion(v => v.Date, s => s.Date);
        builder.Property(game => game.LotSize).IsRequired();
        builder.Property(game => game.Value).IsRequired();

        builder.ToTable("currencies_ticks");
    }
}