using Helium.Values;

namespace Helium.Parser.Expressions;

public class LogarithmicExpression : Expression
{
    public Expression Base;
    public Expression Expr;

    public LogarithmicExpression(Expression @base, Expression expr)
    {
        Base = @base;
        Expr = expr;
    }

    public override ResultValue Evaluate()
    {
        var expr = Expr.Evaluate();
        if (expr is Undefined)
            return expr;
            
        var @base = Base.Evaluate();
        if (@base is Undefined)
            return @base;
        
        return MathV.Log(expr, @base);
    }
}