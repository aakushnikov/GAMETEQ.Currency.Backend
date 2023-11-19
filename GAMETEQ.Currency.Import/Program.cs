using GAMETEQ.Currency.Configuration;
using GAMETEQ.Currency.Extensions;
using GAMETEQ.Currency.Import;
using Microsoft.Extensions.Logging;

try
{
    var settings = new EnvironmentSettings();
    var currencyDbContext = settings.DbConnectionType.CreateDbContext(settings.DbConnectionString);
    var loggerFactory = settings.MinimumLogLevel.GetLoggerFactory("currency-import");
    var logger = loggerFactory.CreateLogger(typeof(Processor));
    var processor = new Processor(settings, logger, currencyDbContext);

    return await processor.Run(args);
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
    return Exit.WithCode(ExitCode.UnhandledError);
}