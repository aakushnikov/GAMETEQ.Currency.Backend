using GAMETEQ.Currency.Data;
using Serilog.Events;

namespace GAMETEQ.Currency.Configuration;

public interface ISettings
{
    DbConnectionType DbConnectionType { get; }
    string DbConnectionString { get; }
    
    string CurrencySourceUrl { get; }
    
    LogEventLevel MinimumLogLevel { get; }
}