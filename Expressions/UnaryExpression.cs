namespace Calculator.Expressions;

public class UnaryExpression : Expression
{
    public UnaryOperator Operator;
    public Expression Expr;

    public UnaryExpression(UnaryOperator @operator, Expression expr)
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
                
                return new DecimalValue(Factorial(d.Value));
            }
        }

        throw new NotImplementedException();
    }
    
    private static double Factorial(double a)
    {
        if (a % 1 == 0)
        {
            var result = 1.0;
            for (int i = 1; i <= a; i++)
            {
                result *= i;
            }
            return result;
        }
        return Gamma(a + 1);
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
        -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7 };
}
