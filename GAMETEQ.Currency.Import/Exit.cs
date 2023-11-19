using System.Collections.Immutable;
using System.Text;

namespace GAMETEQ.Currency.Import;

internal enum ExitCode
{
    Success = 0,
    UnhandledError = -1,
    ImportFailed = -10,
    TooManyArgs = -20,
    ArgNotInt = -21,
    FutureYear = -22,
}

internal static class Exit
{
    private static readonly IImmutableDictionary<ExitCode, string> Codes = new Dictionary<ExitCode, string>()
    {
        { ExitCode.Success, "Import succeeded" },
        { ExitCode.ImportFailed, "Import failed" },
        { ExitCode.TooManyArgs, "Too many arguments. Try specify year either do not specify any arguments" },
        { ExitCode.ArgNotInt, "Specified argument is not a number and cannot be used as year value" },
        { ExitCode.FutureYear, "Import cannot be performed for any future year. Try to specify current or past year" },
    }.ToImmutableDictionary();

    internal static string GetCodesDescription()
    {
        var sb = new StringBuilder();
        
        foreach (var kv in Codes.OrderByDescending(o => o.Key))
        {
            sb.Append('\t');
            sb.Append((int)kv.Key);
            sb.Append(':');
            sb.Append(' ');
            sb.AppendLine(kv.Value);
        }

        return sb.ToString();
    }
    
    internal static int WithCode(ExitCode exitCode)
    {
        if (!Codes.Keys.Contains(exitCode))
            throw new NotImplementedException("No implementation found for specified code");

        var returnCode = (int)exitCode;
    
        Console.WriteLine(Codes[exitCode]);
        return returnCode;
    }
}