using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace GAMETEQ.Currency.Extensions;

public static class LogHelpers
{
    private static ILoggerFactory? _loggerFactory;
    
    public static ILoggerFactory GetLoggerFactory(this LogEventLevel logEventLevel, string logPropertyName)
    {
        if (_loggerFactory is not null)
            return _loggerFactory;
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", logEventLevel)
            .MinimumLevel.Override("System", logEventLevel)
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(GAMETEQ), logPropertyName)
            .WriteTo.Console(
                formatter: new CompactJsonFormatter(),
                restrictedToMinimumLevel: logEventLevel
            )
            .CreateLogger();

        _loggerFactory = LoggerFactory.Create(cfg => cfg.AddSerilog(logger));
        return _loggerFactory;
    }
}