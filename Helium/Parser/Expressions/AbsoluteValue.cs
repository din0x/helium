using Helium.Values;

namespace Helium.Parser.Expressions;

public class AbsoluteValue : Expression
{
    public Expression Expr;

    public AbsoluteValue(Expression expr)
    {
        Expr = expr;
    }

    public override ResultValue Evaluate()
    {
        var expr = Expr.Evaluate();
        if (expr is Undefined)
            return expr;
        
        return MathV.Abs(expr);
    }
}
