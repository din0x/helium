using Helium.Values;

namespace Helium;

public static class MathV
{
    public static readonly ResultValue Pi = new DecimalValue(Math.PI);
    public static readonly ResultValue Tau = new DecimalValue(Math.Tau);
    public static readonly ResultValue E = new DecimalValue(Math.E);
    public static readonly ResultValue Infinity = new InfiniteValue(true);
    
    public static ResultValue Add(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(decimalA.Value + decimalB.Value),
            (DecimalValue, InfiniteValue infinity) => infinity,
            (InfiniteValue infinity, DecimalValue) => infinity,
            (InfiniteValue infinityA, InfiniteValue infinityB) => infinityA.Sign == infinityB.Sign ? infinityA : new Undefined(),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Subtract(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(decimalA.Value - decimalB.Value),
            (DecimalValue, InfiniteValue infinity) => new InfiniteValue(!infinity.Sign),
            (InfiniteValue infinity, DecimalValue) => infinity,
            (InfiniteValue infinityA, InfiniteValue infinityB) => infinityA.Sign == infinityB.Sign ? new Undefined() : infinityA,
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Mulitiply(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(decimalA.Value * decimalB.Value),
            (DecimalValue decimalA, InfiniteValue infinity) => decimalA.Value != 0 ? new InfiniteValue(infinity.Sign == decimalA.Sign) : decimalA,
            (InfiniteValue infinity, DecimalValue decimalB) => decimalB.Value != 0 ? new InfiniteValue(infinity.Sign == decimalB.Sign) : decimalB,
            (InfiniteValue infinityA, InfiniteValue infinityB) => new InfiniteValue(infinityA.Sign == infinityB.Sign),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Divide(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(decimalA.Value / decimalB.Value),
            (DecimalValue, InfiniteValue) => new DecimalValue(0),
            (InfiniteValue infinity, DecimalValue decimalB) => new InfiniteValue(infinity.Sign == decimalB.Sign),
            (InfiniteValue, InfiniteValue) => new Undefined(),
            _ => throw new NotImplementedException()
        };
    }

    public static ResultValue Mod(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(decimalA.Value % decimalB.Value),
            (DecimalValue, InfiniteValue) => new Undefined(),
            (InfiniteValue, DecimalValue) => new Undefined(),
            (InfiniteValue, InfiniteValue) => new Undefined(),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Pow(ResultValue a, ResultValue b)
    {
        return (a, b) switch
        {
            (DecimalValue decimalA, DecimalValue decimalB) => new DecimalValue(Math.Pow(decimalA.Value, decimalB.Value)),
            (DecimalValue decimalA, InfiniteValue infinity) =>
                infinity.Sign ? Math.Abs(decimalA.Value) < 1 ? new DecimalValue(0)
                                                             : decimalA.Value.Equals(1) ? new DecimalValue(1)
                                                                                        : new InfiniteValue(true)
                              : new DecimalValue(0),
            
            (InfiniteValue infinity, DecimalValue decimalB) =>
                decimalB.Value < 0 ? new DecimalValue(0)
                                   : new InfiniteValue(infinity.Sign || decimalB.Value % 2 == 0),
            
            (InfiniteValue, InfiniteValue) => new Undefined(),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Log(ResultValue a, ResultValue newBase)
    {
        return (a, newBase) switch
        {
            (DecimalValue decimalA, DecimalValue decimalBase) => new DecimalValue(Math.Log(decimalA.Value, decimalBase.Value)),
            (DecimalValue decimalA, InfiniteValue) => decimalA.Value != 0 ? new DecimalValue(0) : new Undefined(),
            (InfiniteValue, DecimalValue) => Infinity,
            (InfiniteValue, InfiniteValue) => new Undefined(),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Plus(ResultValue a)
    {
        return a;
    }
    
    public static ResultValue Minus(ResultValue a)
    {
        return a switch
        {
            DecimalValue decimalValue => new DecimalValue(-decimalValue.Value),
            InfiniteValue infinity => new InfiniteValue(!infinity.Sign),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Abs(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(d.Value < 0 ? -d.Value : d.Value),
            InfiniteValue => Infinity,
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Sin(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Sin(d.Value)),
            InfiniteValue => new RangeValue(new DecimalValue(-1), new DecimalValue(1)),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Cos(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Cos(d.Value)),
            InfiniteValue => new RangeValue(new DecimalValue(-1), new DecimalValue(1)),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Tan(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Tan(d.Value)),
            //Infinity => TODO
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Arcsin(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Asin(d.Value)),
            //Infinity => TODO
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Arccos(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Acos(d.Value)),
            //Infinity => TODO
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Arctan(ResultValue a)
    {
        return a switch
        {
            DecimalValue d => new DecimalValue(Math.Atan(d.Value)),
            InfiniteValue i => new DecimalValue(Math.PI / 2 * (i.Sign ? 1 : -1)),
            _ => throw new NotImplementedException()
        };
    }
    
    public static ResultValue Factorial(ResultValue a)
    {
        return a switch
        {
            DecimalValue decimalValue =>
                decimalValue.Value < 0 && decimalValue.Value % 1 == 0 ? decimalValue.Value % 2 == 0 ? Infinity
                                                                     : new InfiniteValue(false)
                                       : new DecimalValue(Factorial(decimalValue.Value)),
            
            InfiniteValue infinity => infinity.Sign ? infinity : new Undefined(),
            _ => throw new NotImplementedException()
        };
    }
    
    private static double Factorial(double a)
    {
        if (a % 1 != 0)
            return Gamma(a + 1);
        
        double result = 1;
        for (int i = 1; i <= a; i++)
        {
            result *= i;
        }
        return result;
    }
    
    private static double Gamma(double z)
    {
        if (z < 0.5)
            return Math.PI / (Math.Sin(Math.PI * z) * Gamma(1 - z));
        z -= 1;
        double x = P[0];
        for (var i = 1; i < G + 2; i++)
            x += P[i] / (z + i);
        double t = z + G + 0.5;
        return Math.Sqrt(2 * Math.PI) * (Math.Pow(t, z + 0.5)) * Math.Exp(-t) * x;
    }

    private static readonly double G = 7;
    private static readonly double[] P = {
        0.99999999999980993, 676.5203681218851, -1259.1392167224028,
        771.32342877765313, -176.61502916214059, 12.507343278686905,
        -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7
    };
}