namespace Calculator.Results;

public class Undefined : Result
{
    public static readonly Undefined Instance = new();
    private static readonly string Name = "undefined";

    public override bool Sign => true;

    public override string ToString()
    {
        return Name;
    }
}