using Calculator.Results;

namespace Calculator.Expresstions;

public class Function : Expression
{
    public string Name;
    public Expression[] Args;
    public Expression? Base;

    public Function(string name, Expression[] args, Expression? @base)
    {
        Name = name;
        Args = args;
        Base = @base;
    }

    public override Result Evaluate()
    {
        if (Name == "log")
        {
            var result = Args[0].Evaluate();

            if (result is Infinity)
                return Infinity.PositiveInfinity;

            else if (result is DecimalValue d)
                return new DecimalValue(Math.Log(d.Value, (Base?.Evaluate() as DecimalValue)?.Value ?? 10));
        }

        if (Name == "ln")
        {
            var result = Args[0].Evaluate();

            if (result is Infinity)
                return Infinity.PositiveInfinity;

            else if (result is DecimalValue d)
                return new DecimalValue(Math.Log(d.Value));
        }

        if (Args[0].Evaluate() is DecimalValue value)
        {
            if (Name == "sin")
                return new DecimalValue(Math.Sin(value.Value));

            if (Name == "cos")
                return new DecimalValue(Math.Cos(value.Value));

            if (Name == "tan")
                return new DecimalValue(Math.Tan(value.Value));
        }

        return Undefined.Instance;
    }

    public static bool IsFunction(string name)
    {
        return _functions.ContainsKey(name);
    }

    private static readonly Dictionary<string, Expression> _functions = new() 
    {
        { "log", new Invalid() },
        { "sin", new Invalid() },
        { "cos", new Invalid() },
        { "tan", new Invalid() },
        { "ln", new Invalid() },
    };
}
