using System.Diagnostics.CodeAnalysis;
using Helium.Values;

namespace Helium;

public sealed class Result
{
    private readonly ResultValue? _successValue;
    private readonly string? _errorMessage;

    public Result(ResultValue successValue)
    {
        _successValue = successValue;
        _errorMessage = null;
    }
    
    public Result(string errorMessage)
    {
        _successValue = null;
        _errorMessage = errorMessage;
    }

    public bool TryGetValue([NotNullWhen(true)] out ResultValue? result, [NotNullWhen(false)] out string? errorMessage)
    {
        result = _successValue;
        errorMessage = _errorMessage;
        return result is not null;
    }
}