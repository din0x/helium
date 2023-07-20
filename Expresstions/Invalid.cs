using Calculator.Results;

namespace Calculator.Expresstions;

public class Invalid : Expression
{
    public override Result Evaluate()
    {   
        throw new NotImplementedException();
    }
}
