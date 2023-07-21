using Calculator.Results;

namespace Calculator.Expressions;

public class Symbol : Expression
{
    public readonly string Name;

    public Symbol(string name)
    {
        Name = name;
    }

    public override Result Evaluate()
    {
        if (Name == "pi")
            return new DecimalValue(3.141592653589793238462643383279);
        if (Name == "e")
            return new DecimalValue(2.718281828459045235360287471352);
        if (Name == "infinity")
            return Infinity.PositiveInfinity;

        return Undefined.Instance;
    }
}
