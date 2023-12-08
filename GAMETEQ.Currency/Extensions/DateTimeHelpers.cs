using System.Globalization;

namespace GAMETEQ.Currency.Extensions;

public static class DateTimeHelpers
{
    public const string DefaultDateTimeFormat = "yyyy-MM-dd";
    
    public static bool TryGetDate(this string date, out DateTime dateTime)
    {
        return DateTime.TryParseExact(date, DefaultDateTimeFormat, null, DateTimeStyles.AssumeUniversal, out dateTime);
    }
}