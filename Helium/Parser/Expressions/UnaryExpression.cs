using Helium.Values;

namespace Helium.Parser.Expressions;

public class UnaryExpression : Expression
{
    public UnaryOperator Operator;
    public Expression Expr;

    public UnaryExpression(UnaryOperator @operator, Expression expr)
    {
        Operator = @operator;
        Expr = expr;
    }
    
    public override ResultValue Evaluate()
    {
        var expr = Expr.Evaluate();
        if (expr is Undefined)
            return expr;
        
        return Operator switch
        {
            UnaryOperator.Plus => MathV.Plus(expr),
            UnaryOperator.Minus => MathV.Minus(expr),
            UnaryOperator.Factorial => MathV.Factorial(expr),
            _ => throw new NotImplementedException()
        };
    }
/*
        if (Operator == UnaryOperator.Factorial)
        {
            var val = Expr.Evaluate();

            if (val is Undefined)
                return new Undefined();

            if (val is Infinity)
                return Infinity.PositiveInfinity;

            if (val is DecimalValue d)
            {
                return Factorial(d.Value);
            }
        }

        throw new NotImplementedException();
    }

    private static Result Factorial(double a)
    {
        if (a < 0 && a % 1 == 0)
            return a % 2 == 0 ? Infinity.PositiveInfinity : Infinity.NegativeInfinity;

        if (a % 1 == 0)
        {
            var result = 1.0;
            for (int i = 1; i <= a; i++)
            {
                result *= i;
            }
            return new DecimalValue(result);
        }
        return new DecimalValue(Gamma(a + 1));
    }
    
    private static double Gamma(double z)
    {
        if (z < 0.5)
            return Math.PI / (Math.Sin(Math.PI * z) * Gamma(1 - z));
        z -= 1;
        double x = P[0];
        for (var i = 1; i < G + 2; i++)
            x += P[i] / (z + i);
        double t = z + G + 0.5;
        return Math.Sqrt(2 * Math.PI) * (Math.Pow(t, z + 0.5)) * Math.Exp(-t) * x;
    }
    
    private static readonly double G = 7;
    private static readonly double[] P = { 0.99999999999980993, 676.5203681218851, -1259.1392167224028,
        771.32342877765313, -176.61502916214059, 12.507343278686905,
        -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7 };*/
}
