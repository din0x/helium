using Helium.Values;

namespace Helium.Parser.Expressions;

public class NumberLiteral : Expression
{
    public readonly double Value;

    public NumberLiteral(double value)
    {
        Value = value;
    }

    public override ResultValue Evaluate()
    {
        return new DecimalValue(Value);
    }
}
