namespace GAMETEQ.Currency.Exceptions;

public sealed class ConfigurationException : ApplicationException
{
    public static ConfigurationException FailedToRead(string parameterName) =>
        new ConfigurationException($"Cannot read configuration parameter '{parameterName}'");

    public ConfigurationException(string message) : base(message) { }
}