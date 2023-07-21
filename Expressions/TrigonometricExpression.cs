namespace Calculator.Expressions;

public class TrigonometricExpression : Expression
{
    public TrigonometricFunctionType Type;
    public Expression Expr;

    public TrigonometricExpression(TrigonometricFunctionType type, Expression expr)
    {
        Type = type;
        Expr = expr;
    }

    public override Result Evaluate()
    {
        var result = Expr.Evaluate();
        if (result is Undefined)
            return result;
        
        if (Type == TrigonometricFunctionType.Sin)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(Math.Sin(decimalResult.Value));
            }
        }
        if (Type == TrigonometricFunctionType.Cos)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(Math.Cos(decimalResult.Value));
            }
        }
        if (Type == TrigonometricFunctionType.Tan)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(Math.Tan(decimalResult.Value));
            }
        }
        if (Type == TrigonometricFunctionType.Cot)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(1 / Math.Tan(decimalResult.Value));
            }
        }
        if (Type == TrigonometricFunctionType.Sec)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(1 / Math.Cos(decimalResult.Value));
            }
        }
        if (Type == TrigonometricFunctionType.Csc)
        {
            if (result is DecimalValue decimalResult)
            {
                return new DecimalValue(1 / Math.Sin(decimalResult.Value));
            }
        }
        
        return Undefined.Instance;
    }
}