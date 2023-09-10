using Helium.Values;

namespace Helium.Parser.Expressions;

public abstract class Expression
{
    public abstract ResultValue Evaluate();
}