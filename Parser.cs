using Calculator.Expresstions;

namespace Calculator;

public class Parser
{
    public Expression Expr { get; private set; }

    public Parser(Token[] tokens)
    {
        _tokens = new List<Token>(tokens);
        Expr = Parse();
    }

    private readonly List<Token> _tokens;

    private Expression Parse()
    {
        return ParseBinary();
    }

    private Expression ParseBinary()
    {
        return ParseAdditive();
    }

    private Expression ParseAdditive()
    {
        var left = ParseMultiplicative();

        while ((At().Value == "+" || At().Value == "-") && NotEOF)
        {
            var opmap = new Dictionary<string, BinaryOperator>()
            {
                { "+", BinaryOperator.Add },
                { "-", BinaryOperator.Subtract },
            };

            var op = opmap[Eat().Value];

            var right = ParseMultiplicative();

            left = new Binary(op, left, right);
        }

        return left;
    }

    private Expression ParseMultiplicative()
    {
        var left = ParsePow();

        while ((At().Value == "*" || At().Value == "/" || At().Value == "%") && NotEOF)
        {
            var opmap = new Dictionary<string, BinaryOperator>()
            {
                { "*", BinaryOperator.Multiply },
                { "/", BinaryOperator.Divide },
                { "%", BinaryOperator.Mod },
            };

            var op = opmap[Eat().Value];

            var right = ParsePow();

            left = new Binary(op, left, right);
        }

        return left;
    }

    private Expression ParsePow()
    {
        var left = ParseUnary();

        while (At().Value == "^" && NotEOF)
        {
            Eat();

            var right = ParseUnary();

            left =  new Binary(BinaryOperator.Pow, left, right);
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

        return new Unary(op, left);
    }

    private Expression ParseImplicitMultipication()
    {
        var left = ParseFunction();

        if (At().Type == TokenType.Symbol
            || At().Type == TokenType.Number
            || At().Type == TokenType.OpenParen)
        {
            left = new Binary(BinaryOperator.Multiply, left, ParseFunction());
        }

        return left;
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
                args.Add(Parse());
                while (At().Type == TokenType.Comma)
                {
                    Eat();
                    args.Add(Parse());
                }
                if (At().Type != TokenType.CloseParen)
                    return new Invalid();
            }
            else
            {
                args.Add(Parse());
            }

            Expression fn = new Function(name, args.ToArray(), @base);

            if (exp is not null)
                fn = new Binary(BinaryOperator.Pow, fn, exp);

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
            expr = new Unary(UnaryOperator.Factorial, expr);
        }

        return expr;
    }

    private Expression ParsePrimary()
    {
        Expression result;

        if (At().Type == TokenType.Number)
        {
            result = new Number(double.Parse(Eat().Value));
        }
        else if (At().Type == TokenType.Symbol)
        {
            result = new Symbol(Eat().Value);
        }
        else if (At().Type == TokenType.OpenParen)
        {
            Eat();
            var expr = Parse();

            if (At().Type != TokenType.CloseParen)
                return new Invalid();

            Eat();
            result = expr;
        }
        else if (At().Type == TokenType.Pipe)
        {
            Eat();
            var expr = Parse();
            if (At().Type != TokenType.Pipe)
                return new Invalid();
            Eat();
            result = new Absolute(expr);
        }

        else
        {
            return new Invalid();
        }

        return result;
    }

    private bool NotEOF => _tokens.Count > 0 && _tokens[0].Type != TokenType.End;
    
    private Token At()
    {
        if (NotEOF)
            return _tokens[0];
        return new Token(TokenType.End, "");
    }

    private Token Eat()
    {
        var at = At();
        if (NotEOF)
            _tokens.RemoveAt(0);
        
        return at;
    }
}
