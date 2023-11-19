using GAMETEQ.Currency.Data;
using GAMETEQ.Currency.Exceptions;
using Serilog.Events;

namespace GAMETEQ.Currency.Configuration;

public sealed class EnvironmentSettings : ISettings
{
    public DbConnectionType DbConnectionType { get; } = GetValueAsEnum<DbConnectionType>(nameof(DbConnectionType));
    public string DbConnectionString { get; } = GetValue<string>(nameof(DbConnectionString));
    
    public string CurrencySourceUrl { get; } = GetValue<string>(nameof(CurrencySourceUrl));
    
    public LogEventLevel MinimumLogLevel { get; } = GetValueAsEnum<LogEventLevel>(nameof(MinimumLogLevel));

    private static T GetValue<T>(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);

        if (value is null)
            throw new ConfigurationException(name);

        dynamic obj = value;

        switch (obj)
        {
            case string _:
                return (T)obj;

            case int _:
                int.TryParse(obj, out int i);
                return (T)(dynamic)i;

            case long _:
                long.TryParse(obj, out long l);
                return (T)(dynamic)l;

            case decimal _:
                decimal.TryParse(obj, out decimal d);
                return (T)(dynamic)d;

            case bool _:
                bool.TryParse(obj, out bool b);
                return (T)(dynamic)b;

            default:
                throw new NotImplementedException($"There was no implementation found for type: {typeof(T)}");
        }
    }
    
    private static T GetValueAsEnum<T>(string name) where T : struct, Enum
    {
        var value = GetValue<string>(name);

        if (string.IsNullOrWhiteSpace(value))
            throw new ConfigurationException(name);

        return Enum.Parse<T>(value, true);
    }
}
