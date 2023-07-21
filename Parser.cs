using Calculator.Expressions;

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
        var left = ParsePow();

        while ((At().Value == "*" || At().Value == "/" || At().Value == "%") && NotEof)
        {
            var opmap = new Dictionary<string, BinaryOperator>()
            {
                { "*", BinaryOperator.Multiply },
                { "/", BinaryOperator.Divide },
                { "%", BinaryOperator.Mod },
            };

            var op = opmap[Eat().Value];

            var right = ParsePow();

            left = new BinaryExpression(op, left, right);
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
            return ParseImplicitMultipication();

        var opmap = new Dictionary<string, UnaryOperator>()
        {
            { "+", UnaryOperator.Plus },
            { "-", UnaryOperator.Minus },
        };
        var op = opmap[Eat().Value];
        var left = ParseImplicitMultipication();

        return new UnaryExpression(op, left);
    }

    private Expression ParseImplicitMultipication()
    {
        var left = ParseAdvancedExpression();

        if (At().Type == TokenType.Symbol
            || At().Type == TokenType.Number
            || At().Type == TokenType.OpenParen)
        {
            left = new BinaryExpression(BinaryOperator.Multiply, left, ParseAdvancedExpression());
        }

        return left;
    }

    private Expression ParseAdvancedExpression()
    {
        if (At().Value == "log")
        {
            Eat();
            Expression @base = new NumberLiteral(10);
            if (At().Type == TokenType.Base)
            {
                Eat();
                @base = ParsePrimary();
            }
            
            var expr = ParseUnary();
            
            return new LogarithmicExpression(@base, expr);
        }
        if (At().Value == "ln")
        {
            Eat();
            var expr = ParseUnary();
            
            return new LogarithmicExpression(new NumberLiteral(Math.E), expr);
        }
        if (At().Value == "lg")
        {
            Eat();
            var expr = ParseUnary();
    
            return new LogarithmicExpression(new NumberLiteral(10), expr);
        }
        
        return ParseFunction();
    }
    
    private Expression ParseFunction()
    {
        if (At().Type == TokenType.Symbol && Function.IsFunction(At().Value))
        {
            var name = Eat().Value;

            Expression? @base = null;

            if (At().Type == TokenType.Base)
            {
                Eat();
                @base = ParsePrimary();
            }

            Expression? exp = null;
            if (At().Value == "^")
            {
                Eat();
                exp = ParsePrimary();
            }

            var args = new List<Expression>();
            if (At().Type == TokenType.OpenParen)
            {
                Eat();
                args.Add(ParseExpression());
                while (At().Type == TokenType.Comma)
                {
                    Eat();
                    args.Add(ParseExpression());
                }
                if (At().Type != TokenType.CloseParen)
                    return new InvalidExpression();
            }
            else
            {
                args.Add(ParseExpression());
            }

            Expression fn = new Function(name, args.ToArray(), @base);

            if (exp is not null)
                fn = new BinaryExpression(BinaryOperator.Pow, fn, exp);

            return fn;
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
        Expression result;

        if (At().Type == TokenType.Number)
        {
            result = new NumberLiteral(double.Parse(Eat().Value));
        }
        else if (At().Type == TokenType.Symbol)
        {
            result = new Symbol(Eat().Value);
        }
        else if (At().Type == TokenType.OpenParen)
        {
            Eat();
            var expr = ParseExpression();

            if (At().Type != TokenType.CloseParen)
                return new InvalidExpression();

            Eat();
            result = expr;
        }
        else if (At().Type == TokenType.Pipe)
        {
            Eat();
            var expr = ParseExpression();
            if (At().Type != TokenType.Pipe)
                return new InvalidExpression();
            Eat();
            result = new AbsoluteValue(expr);
        }

        else
        {
            Invalid = true;
            return new InvalidExpression();
        }

        return result;
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
