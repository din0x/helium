using Calculator.Results;

namespace Calculator.Expresstions;

public class Unary : Expression
{
    public UnaryOperator Operator;
    public Expression Expr;

    public Unary(UnaryOperator @operator, Expression expr)
    {
        Operator = @operator;
        Expr = expr;
    }

    public override Result Evaluate()
    {
        if (Operator == UnaryOperator.Plus)
        {
            return Expr.Evaluate();
        }

        if (Operator == UnaryOperator.Minus)
        {
            var val = Expr.Evaluate();

            if (val is Undefined)
                return Undefined.Instance;

            if (val is Infinity i)
                return i.Sign ? Infinity.NegativeInfinity : Infinity.PositiveInfinity;

            if (val is DecimalValue d)
                return new DecimalValue(-d.Value);
        }

        if (Operator == UnaryOperator.Factorial)
        {
            var val = Expr.Evaluate();

            if (val is Undefined)
                return new Undefined();

            if (val is Infinity)
                return Infinity.PositiveInfinity;

            if (val is DecimalValue d)
            {
                if (d.Value < 0)
                    return Infinity.PositiveInfinity;

                double result = 1;

                for (int i = 1; i <= d.Value; i++)
                {
                    result *= i;
                }

                return new DecimalValue(result);
            }
        }

        throw new NotImplementedException();
    }
}
