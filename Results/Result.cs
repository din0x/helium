namespace Calculator.Results;

public abstract class Result
{
    public abstract bool Sign { get; }

    public abstract override string ToString();
}