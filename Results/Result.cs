namespace Calculator.Results;

public abstract class Result
{
    public abstract bool Sign { get; }

    public override abstract string ToString();
}
