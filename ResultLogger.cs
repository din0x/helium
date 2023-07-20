using Calculator.Results;

namespace Calculator;

internal static class ResultLogger
{
    public static void Log(Result result)
    {
        var str = result.ToString();
        Console.Write("> ");
        RawLog(str, result is Undefined ? ConsoleColor.DarkGray : ConsoleColor.Gray);
    }

    public static void LogWarning(string message)
    {
        RawLog(message, ConsoleColor.Yellow);
    }

    public static void LogError(string error)
    {
        RawLog(error, ConsoleColor.Red);
    }

    private static void RawLog(string message, ConsoleColor color)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = previousColor;
    }
}