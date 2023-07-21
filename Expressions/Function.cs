namespace Calculator.Expressions;

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
        return Functions.ContainsKey(name);
    }

    private static readonly Dictionary<string, Expression> Functions = new() 
    {
        { "log", new InvalidExpression() },
        { "sin", new InvalidExpression() },
        { "cos", new InvalidExpression() },
        { "tan", new InvalidExpression() },
        { "ln", new InvalidExpression() },
    };
}
