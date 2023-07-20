using Calculator.Results;

namespace Calculator.Expressions;

public class NumberLiteral : Expression
{
    public readonly double Value;

    public NumberLiteral(double value)
    {
        Value = value;
    }

    public override Result Evaluate()
    {
        return new DecimalValue(Value);
    }
}
