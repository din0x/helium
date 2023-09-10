using Helium.Values;

namespace Helium.Parser.Expressions;

public class InvalidExpression : Expression
{
    public override ResultValue Evaluate()
    {
        throw new NotImplementedException("Cannot evaluate an invalid expression");
    }
}
