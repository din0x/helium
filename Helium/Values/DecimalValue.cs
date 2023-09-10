namespace Helium.Values;

public sealed class DecimalValue : ResultValue
{
    public double Value { get; }
    public bool Sign => Value >= 0;
    
    public DecimalValue(double value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString("g").Replace('E', 'e').Replace(',', '.');
    }
}