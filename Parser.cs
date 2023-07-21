namespace Calculator;

public class Parser
{
    public Expression Expr { get; private set; }
    public bool Invalid { get; private set; }
    public Parser(Token[] tokens)
    {
        Invalid = false;
        _tokens = new List<Token>(tokens);
        Expr = Parse();
    }

    private readonly List<Token> _tokens;

    private Expression Parse()
    {
        var expr = ParseExpression();
        if (NotEof)
        {
            Invalid = true;
            return new InvalidExpression();
        }
        return  expr;
    }
    
    private Expression ParseExpression()
    {
        return ParseBinaryExpression();
    }

    private Expression ParseBinaryExpression()
    {
        return ParseAdditive();
    }

    private Expression ParseAdditive()
    {
        var left = ParseMultiplicative();

        while ((At().Value == "+" || At().Value == "-") && NotEof)
        {
            var opmap = new Dictionary<string, BinaryOperator>()
            {
                { "+", BinaryOperator.Add },
                { "-", BinaryOperator.Subtract },
            };

            var op = opmap[Eat().Value];

            var right = ParseMultiplicative();

            left = new BinaryExpression(op, left, right);
        }

        return left;
    }

    private Expression ParseMultiplicative()
    {
        var left = ParseImplicitMultipication();

        while ((At().Value == "*" || At().Value == "/" || At().Value == "%") && NotEof)
        {
            var opmap = new Dictionary<string, BinaryOperator>()
            {
                { "*", BinaryOperator.Multiply },
                { "/", BinaryOperator.Divide },
                { "%", BinaryOperator.Mod },
            };

            var op = opmap[Eat().Value];

            var right = ParseImplicitMultipication();

            left = new BinaryExpression(op, left, right);
        }

        return left;
    }

    private Expression ParseImplicitMultipication()
    {
        var left = ParsePow();

        if (At().Type == TokenType.Symbol
            || At().Type == TokenType.Number
            || At().Type == TokenType.OpenParen)
        {
            left = new BinaryExpression(BinaryOperator.Multiply, left, ParsePow());
        }

        return left;
    }
    
    private Expression ParsePow()
    {
        var left = ParseUnary();

        while (At().Value == "^" && NotEof)
        {
            Eat();

            var right = ParseUnary();

            left =  new BinaryExpression(BinaryOperator.Pow, left, right);
        }

        return left;
    }

    private Expression ParseUnary()
    {
        if (At().Value != "+" && At().Value != "-")
            return ParseAdvancedExpression();

        var opmap = new Dictionary<string, UnaryOperator>()
        {
            { "+", UnaryOperator.Plus },
            { "-", UnaryOperator.Minus },
        };
        var op = opmap[Eat().Value];
        var left = ParseAdvancedExpression();

        return new UnaryExpression(op, left);
    }

    private Expression ParseAdvancedExpression()
    {
        if (At().Type != TokenType.Symbol || !(At().Value is "log" or "ln" or "lg" or "sin" or "cos" or "tan" or "cot" or "sec" or "csc"))
            return ParseFactorial();
        
        var name = Eat().Value;
        
        if (name == "log")
        {
            Expression @base = new NumberLiteral(10);
            if (At().Type == TokenType.Base)
            {
                Eat();
                @base = ParsePrimary();
            }
           
            var expr = ParseUnary();
            
            return new LogarithmicExpression(@base, expr);
        }
        if (name == "ln")
        {
            var expr = ParseUnary();
            
            return new LogarithmicExpression(new NumberLiteral(Math.E), expr);
        }
        if (name == "lg")
        {
            var expr = ParseUnary();
    
            return new LogarithmicExpression(new NumberLiteral(10), expr);
        }
        if (name is "sin" or "cos" or "tan" or "cot" or "sec" or "csc")
        {
            var map = new Dictionary<string, TrigonometricFunctionType>()
            {
                { "sin", TrigonometricFunctionType.Sin },
                { "cos", TrigonometricFunctionType.Cos },
                { "tan", TrigonometricFunctionType.Tan },
                { "cot", TrigonometricFunctionType.Cot },
                { "sec", TrigonometricFunctionType.Sec },
                { "csc", TrigonometricFunctionType.Csc },
            };
            
            var type = map[name];
            var expr = ParseUnary();
            
            return new TrigonometricExpression(type, expr);
        }
        
        return ParseFactorial();
    }
    
    private Expression ParseFactorial()
    {
        var expr = ParsePrimary();

        if (At().Value == "!")
        {
            Eat();
            expr = new UnaryExpression(UnaryOperator.Factorial, expr);
        }

        return expr;
    }

    private Expression ParsePrimary()
    {
        if (At().Type == TokenType.Number)
        {
            return new NumberLiteral(double.Parse(Eat().Value));
        }

        if (At().Type == TokenType.Symbol)
        {
            return new Symbol(Eat().Value);
        }

        if (At().Type == TokenType.OpenParen)
        {
            Eat();
            var expr = ParseExpression();

            if (At().Type != TokenType.CloseParen)
                return new InvalidExpression();

            Eat();
            return expr;
        }
        
        if (At().Type == TokenType.Pipe)
        {
            Eat();
            var expr = ParseExpression();
            if (At().Type != TokenType.Pipe)
                return new InvalidExpression();
            Eat();
            return new AbsoluteValue(expr);
        }

        Invalid = true;
        Console.WriteLine(At().Value);
        return new InvalidExpression();
    }

    private bool NotEof => _tokens.Count > 0 && _tokens[0].Type != TokenType.End;
    
    private Token At()
    {
        if (NotEof)
            return _tokens[0];
        return new Token(TokenType.End, "");
    }

    private Token Eat()
    {
        var at = At();
        if (NotEof)
            _tokens.RemoveAt(0);
        
        return at;
    }
}
