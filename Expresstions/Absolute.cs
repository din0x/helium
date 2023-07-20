using Calculator.Results;

namespace Calculator.Expresstions;

public class Absolute : Expression
{
    public Expression Expr;

    public Absolute(Expression expr)
    {
        Expr = expr;
    }

    public override Result Evaluate()
    {
        var result = Expr.Evaluate();

        if (result is Undefined)
            return Undefined.Instance;

        if (result is Infinity)
            return Infinity.PositiveInfinity;

        if (result is DecimalValue d)
            return d.Value < 0 ? new DecimalValue(-d.Value) : d;

        throw new NotImplementedException();
    }
}
