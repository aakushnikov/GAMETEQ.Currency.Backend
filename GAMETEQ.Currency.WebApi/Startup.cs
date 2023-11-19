using GAMETEQ.Currency.Configuration;
using GAMETEQ.Currency.Extensions;

namespace GAMETEQ.Currency.WebApi;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        
        
        var settings = new EnvironmentSettings();
        services.AddSingleton<ISettings>(settings);

        services.AddDbContext(settings.DbConnectionType, settings.DbConnectionString);
    }
}