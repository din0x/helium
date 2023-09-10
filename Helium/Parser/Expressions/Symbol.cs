using Helium.Values;

namespace Helium.Parser.Expressions;

public class Symbol : Expression
{
    public readonly string Name;

    public Symbol(string name)
    {
        Name = name;
    }

    public override ResultValue Evaluate()
    {
        return Name switch
        {
            "pi" => MathV.Pi,
            "tau" => MathV.Tau,
            "e" => MathV.E,
            "infinity" => MathV.Infinity,
            _ => new Undefined()
        };
    }
}
