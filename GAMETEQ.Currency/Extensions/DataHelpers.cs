using GAMETEQ.Currency.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GAMETEQ.Currency.Extensions
{
    public static class DataHelpers
	{
		private static DbContextOptionsBuilder UseDbByType(this DbContextOptionsBuilder options, DbConnectionType dbConnectionType, string connectionString)
		{
			switch (dbConnectionType)
			{
				case DbConnectionType.Sqlite:
					return options.UseSqlite(connectionString);
				case DbConnectionType.Postgres:
					return options.UseNpgsql(connectionString);
				default:
					throw new NotImplementedException($"There was no implementation found for {dbConnectionType}");
			}
		}
		
		public static IServiceCollection AddDbContext(this IServiceCollection services, DbConnectionType dbConnectionType, string connectionString)
		{
			services.AddDbContext<CurrencyDbContext>(options =>
			{
				options.UseDbByType(dbConnectionType, connectionString);
				options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			});
			services.AddScoped<ICurrencyDbContext>(provider =>
				provider.GetService<CurrencyDbContext>()
				?? throw new ApplicationException($"Cannot get injected {nameof(CurrencyDbContext)}"));

			return services;
		}
		
		public static CurrencyDbContext CreateDbContext(this DbConnectionType dbConnectionType, string connectionString)
		{
			var options = new DbContextOptionsBuilder<CurrencyDbContext>()
				.UseDbByType(dbConnectionType, connectionString)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.Options;
        
			var context = new CurrencyDbContext(options);
			context.Database.EnsureCreated();
        
			return context;
		}
	}
}