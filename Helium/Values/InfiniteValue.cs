namespace Helium.Values;

public class InfiniteValue : ResultValue
{
    public bool Sign { get; }

    public InfiniteValue(bool sign)
    {
        Sign = sign;
    }
    
    public override string ToString()
    {
        return $"{(Sign ? "" : "-")}infinity";
    }
}