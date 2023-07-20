namespace Calculator.Results;

public class DecimalValue : Result
{
    public readonly double Value;

    public DecimalValue(double value)
    {
        Value = value;
    }

    public override bool Sign => Value >= 0;

    public override string ToString()
    {
        return Value.ToString().Replace(',', '.');
    }
}