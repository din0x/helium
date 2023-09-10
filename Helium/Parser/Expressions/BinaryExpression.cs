using Helium.Values;

namespace Helium.Parser.Expressions;

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

    public override ResultValue Evaluate()
    {
        var left = Left.Evaluate();
        if (left is Undefined)
            return left;
        
        var right = Right.Evaluate();
        if (right is Undefined)
            return right;
        
        return Operator switch
        {
            BinaryOperator.Add => MathV.Add(left, right),
            BinaryOperator.Subtract => MathV.Subtract(left, right),
            BinaryOperator.Multiply => MathV.Mulitiply(left, right),
            BinaryOperator.Divide => MathV.Divide(left, right),
            BinaryOperator.Mod => MathV.Mod(left, right),
            BinaryOperator.Pow => MathV.Pow(left, right),
            _ => throw new NotImplementedException()
        };
    }
}
