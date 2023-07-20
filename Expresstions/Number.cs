using Calculator.Results;

namespace Calculator.Expresstions;

public class Number : Expression
{
    public readonly double Value;

    public Number(double value)
    {
        Value = value;
    }

    public override Result Evaluate()
    {
        return new DecimalValue(Value);
    }
}
