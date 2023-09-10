using Helium.Lexer;
using Helium.Parser.Expressions;

namespace Helium.Parser;

public class Parser : IParser
{
    private IEnumerator<Token>? _tokens;
    
    public Expression Parse(IEnumerable<Token> tokens)
    {
        _tokens = tokens.GetEnumerator();
        _tokens.MoveNext();
/*        _tokens = tokens.GetEnumerator();
        _tokens.MoveNext();
        At = _tokens.Current;*/
        return ParseExpression();
    }
    
    private bool NotEof => At.Type != TokenType.Eof;

    private Token At => _tokens?.Current ?? throw new Exception("Cannot use 'At' outside 'ParseExpression()'");

    private Token Eat()
    {
        var at = At;
        
        if (_tokens?.MoveNext() ?? throw new Exception("Cannot use 'Eat()' outside 'ParseExpression()'"))
        {
            
        }
        
        return at;
    }
    
    private Expression ParseExpression()
    {
        return ParseAdditive();
    }

    private Expression ParseAdditive()
    {
        var left = ParseMultiplicative();

        while ((At.Value == "+" || At.Value == "-") && NotEof)
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

        while ((At.Value == "*" || At.Value == "/" || At.Value == "%") && NotEof)
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

        if (At.Type == TokenType.Symbol
            || At.Type == TokenType.Number
            || At.Type == TokenType.OpenParen)
        {
            left = new BinaryExpression(BinaryOperator.Multiply, left, ParsePow());
        }

        return left;
    }

    private Expression ParsePow()
    {
        var left = ParseUnary();
    
        while (At.Value == "^" && NotEof)
        {
            Eat();
    
            var right = ParseUnary();
    
            left =  new BinaryExpression(BinaryOperator.Pow, left, right);
        }
    
        return left;
    }
    
    private Expression ParseUnary()
    {
        if (At.Value != "+" && At.Value != "-")
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
        if (At.Type != TokenType.Symbol || !(At.Value is
            "log" or "ln" or "lg" or "sin" or "cos" or "tan" or "arcsin" or "arcsin" or "arctan"))
            return ParseFactorial();
    
        var name = Eat().Value;
        Expression? power = null;
        Expression? result = null;
    
        if (At.Value == "^")
        {
            Eat();
            power = ParseUnary();
        }
    
        if (name == "log")
        {
            Expression @base = new NumberLiteral(10);
            if (At.Type == TokenType.Base)
            {
                Eat();
                @base = ParsePrimary();
            }
    
            var expr = ParseUnary();
        
            result = new LogarithmicExpression(@base, expr);
        }
        if (name == "ln")
        {
            var expr = ParseUnary();
    
            result = new LogarithmicExpression(new NumberLiteral(Math.E), expr);
        }
        if (name == "lg")
        {
            var expr = ParseUnary();
    
            result = new LogarithmicExpression(new NumberLiteral(10), expr);
        }
        if (name is "sin" or "cos" or "tan" or "arcsin" or "arccos" or "arctan")
        {
            var map = new Dictionary<string, TrigonometricFunctionType>()
            {
                { "sin", TrigonometricFunctionType.Sin },
                { "cos", TrigonometricFunctionType.Cos },
                { "tan", TrigonometricFunctionType.Tan },
                { "arcsin", TrigonometricFunctionType.Arcsin },
                { "arccos", TrigonometricFunctionType.Arccos },
                { "arctan", TrigonometricFunctionType.Arctan },
            };
        
        var type = map[name];
        var expr = ParseUnary();
        
        result = new TrigonometricExpression(type, expr);
        }
    
        if (power is not null)
            result = new BinaryExpression(BinaryOperator.Pow, result ?? throw new NotImplementedException(), power);
    
        return result ?? throw new NotImplementedException();
    }
    
    private Expression ParseFactorial()
    {
        var expr = ParsePrimary();
    
        if (At.Value == "!")
        {
            Eat();
            expr = new UnaryExpression(UnaryOperator.Factorial, expr);
        }
    
        return expr;
    }
    
    private Expression ParsePrimary()
    {
        if (At.Type == TokenType.Number)
        {
            return new NumberLiteral(double.Parse(Eat().Value));
        }
    
        if (At.Type == TokenType.Symbol)
        {
            return new Symbol(Eat().Value);
        }
    
        if (At.Type == TokenType.OpenParen)
        {
            Eat();
            var expr = ParseExpression();
    
            if (At.Type != TokenType.CloseParen)
                return new InvalidExpression();
    
            Eat();
            return expr;
        }
    
        if (At.Type == TokenType.Pipe)
        {
            Eat();
            var expr = ParseExpression();
            if (At.Type != TokenType.Pipe)
                return new InvalidExpression();
            Eat();
            return new AbsoluteValue(expr);
        }
        
        //Invalid = true;
        //Console.WriteLine(At.Value);
        return new InvalidExpression();
    }
}