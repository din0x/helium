namespace Calculator.Expressions;

public class LogarithmicExpression : Expression
{
    public Expression Base;
    public Expression Expr;

    public LogarithmicExpression(Expression @base, Expression expr)
    {
        Base = @base;
        Expr = expr;
    }

    public override Result Evaluate()
    {
        var @base = Base.Evaluate();
        if (@base is Undefined)
            return  Undefined.Instance;
        
        var expr = Expr.Evaluate();
        if (expr is Undefined)
            return  Undefined.Instance;
        
        if (@base is Infinity)
        {
            if (expr is Infinity)
                return  Undefined.Instance;
            
            if (expr is DecimalValue)
                return new DecimalValue(0);
        }
        
        if (@base is DecimalValue decimalBase)
        {
            if (expr is Infinity)
                return Infinity.PositiveInfinity;
            
            if (expr is DecimalValue decimalExpr)
            {
                return new DecimalValue(Math.Log(decimalExpr.Value, decimalBase.Value));
            }
        }
        
        return Undefined.Instance;
    }
}