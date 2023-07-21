namespace Calculator.Expressions;

public class BinaryExpression : Expression
{
    public readonly BinaryOperator Operator;
    public readonly Expression Left;
    public readonly Expression Right;

    public BinaryExpression(BinaryOperator @operator, Expression left,  Expression right)
    {
        Operator = @operator;
        Left = left;
        Right = right;
    }

    public override Result Evaluate()
    {
        var left = Left.Evaluate();
        if (left is Undefined)
            return Undefined.Instance;

        var right = Right.Evaluate();
        if (right is Undefined) 
            return Undefined.Instance;

        if (Operator == BinaryOperator.Add)
        {
            if (left is Infinity || right is Infinity)
                return Undefined.Instance;

            if (left is DecimalValue l && right is DecimalValue r)
                return new DecimalValue(l.Value + r.Value);
        }

        if (Operator == BinaryOperator.Subtract)
        {
            if (left is Infinity)
                return right is Infinity ? Undefined.Instance : Infinity.PositiveInfinity;

            if (left is DecimalValue l)
                return right is DecimalValue r ? new DecimalValue(l.Value - r.Value) : Infinity.NegativeInfinity;
        }

        if (Operator == BinaryOperator.Multiply)
        {
            if (left is Infinity li && right is Infinity ri)
                return li.Sign == ri.Sign ? Infinity.PositiveInfinity 
                                          : Infinity.NegativeInfinity;

            if (left is DecimalValue l && right is DecimalValue r) 
                return new DecimalValue(l.Value * r.Value);

            if ((left is DecimalValue dl && dl.Value == 0) || (right is DecimalValue dr && dr.Value == 0))
                return Undefined.Instance;

            return left.Sign == right.Sign ? Infinity.PositiveInfinity
                                           : Infinity.NegativeInfinity;
        }

        if (Operator == BinaryOperator.Divide)
        {
            if (right is DecimalValue dr && dr.Value == 0)
                return Undefined.Instance;

            if (left is Infinity && right is Infinity)
                return Undefined.Instance;

            if (right is Infinity)
                return new DecimalValue(0);

            if (left is Infinity)
                return left;

            if (left is DecimalValue l && right is DecimalValue r)
                return new DecimalValue(l.Value / r.Value);
        }

        if (Operator == BinaryOperator.Mod)
        {
            if (left is Infinity || right is Infinity)
                return Undefined.Instance;

            if (left is DecimalValue l && right is DecimalValue r)
                return new DecimalValue(l.Value % r.Value);
        }

        if (Operator == BinaryOperator.Pow)
        {
            if (right is DecimalValue dr && dr.Value == 0)
                return new DecimalValue(1);

            if (left is DecimalValue dl && dl.Value == 0)
                return new DecimalValue(0);

            if (right is Infinity ir && !ir.Sign)
                return new DecimalValue(0);

            if (left is Infinity)
                return left;

            if (left is DecimalValue l && right is DecimalValue r)
                return new DecimalValue(Math.Pow(l.Value, r.Value));
        }
 
        throw new NotImplementedException();
    }
}
