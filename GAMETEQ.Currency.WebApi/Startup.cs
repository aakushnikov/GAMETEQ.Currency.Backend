using System.Reflection;
using GAMETEQ.Currency.Configuration;
using GAMETEQ.Currency.Extensions;
using GAMETEQ.Currency.WebApi.Middleware;

namespace GAMETEQ.Currency.WebApi;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        var settings = new EnvironmentSettings();
        
        services.AddSingleton<ISettings>(settings);

        services.AddDbContext(settings.DbConnectionType, settings.DbConnectionString);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddControllers();
        
        services.AddSwaggerGen();
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseCustomExceptionHandler<CustomExceptionHandlerMiddleware>();

        app.UseCors("AllowAll");
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
        
        app.UseHttpsRedirection();
        
        app.MapControllers();
    }
}