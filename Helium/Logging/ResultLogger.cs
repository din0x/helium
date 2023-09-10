using Helium.Values;

namespace Helium.Logging;

public class ConsoleLogger : IResultLogger
{
    public void Log(Result result)
    {
        if (result.TryGetValue(out ResultValue? success, out string? errorMessage))
        {
            Console.WriteLine(success);
        }
        else
        {
            Console.WriteLine(errorMessage);
        }
    }
}