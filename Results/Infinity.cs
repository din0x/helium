namespace Calculator.Results;

public class Infinity : Result
{
    public static readonly Infinity PositiveInfinity = new(true);
    public static readonly Infinity NegativeInfinity = new(false);

    private static readonly string Name = "infinity";

    private readonly bool _sign;
    public override bool Sign => _sign;

    private Infinity(bool sign)
    {
        _sign = sign;
    }

    public override string ToString()
    {
        return $"{(_sign ? "" : "-")}{Name}";
    }
}
