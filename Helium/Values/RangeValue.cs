namespace Helium.Values;

public class RangeValue : ResultValue
{
    public ResultValue Min { get; }
    public ResultValue Max { get; }

    public RangeValue(ResultValue min, ResultValue max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString()
    {
        return $"{Min} <= .. <= {Max}";
    }
}