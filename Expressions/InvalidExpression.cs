using Calculator.Results;

namespace Calculator.Expressions;

public class InvalidExpression : Expression
{
    public override Result Evaluate()
    {   
        throw new NotImplementedException();
    }
}
