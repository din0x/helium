using Helium.Values;

namespace Helium.Parser.Expressions;

public class TrigonometricExpression : Expression
{
    public TrigonometricFunctionType Type;
    public Expression Expr;

    public TrigonometricExpression(TrigonometricFunctionType type, Expression expr)
    {
        Type = type;
        Expr = expr;
    }

    public override ResultValue Evaluate()
    {
        var expr = Expr.Evaluate();
        if (expr is Undefined)
            return expr;
        
        return Type switch
        {
            TrigonometricFunctionType.Sin => MathV.Sin(expr),
            TrigonometricFunctionType.Cos => MathV.Cos(expr),
            TrigonometricFunctionType.Tan => MathV.Tan(expr),
            TrigonometricFunctionType.Arcsin => MathV.Arcsin(expr),
            TrigonometricFunctionType.Arccos => MathV.Arccos(expr),
            TrigonometricFunctionType.Arctan => MathV.Arctan(expr),
            _ => throw new NotImplementedException()
        };
    }
}