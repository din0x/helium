namespace Helium.Lexer;

public sealed class Lexer : ILexer
{
    private string _text = "";
    
    public IEnumerable<Token> Tokenize(string text)
    {
        _text = text;
        
        while (true)
        {
            var tok = ParseNumber();
            yield return tok;

            if (tok.Type == TokenType.Eof)
                break;
        }
    }
    
    private Token ParseNumber()
    {
        if (!char.IsNumber(At)) 
            return ParseSymbol();
        
        int i = 0;
        bool usedDot = false;
        bool usedE = false;
        bool usedPlusOrMinus = false;
        string number = "" ;
        while (NotEof
            && (char.IsNumber(At)
            || (At == '.' && i > 0 && !usedDot))
            || (At == 'e' && i > 0 && !usedE)
            || (At is '+' or '-' && usedE && !usedPlusOrMinus))
        {
            if (char.IsNumber(At))
            {
                number += Eat();
            }
                
            else if (At == '.')
            {
                Eat();
                number += ',';
                usedDot = true;
            }
            
            else if (At == 'e')
            {
                Eat();
                number += 'E';
                usedDot = true;
                usedE = true;
            }
            
            else if (At is '+' or '-')
            {
                number += Eat();
                usedPlusOrMinus = true;
            }
            
            i++;
        }
        
        return new Token(TokenType.Number, number.Replace('.', ','));
    }

    private Token ParseSymbol()
    {
        bool AtChar()
        {
            var atbyte = (byte)At;
            return atbyte is > 64 and < 91 || atbyte is > 96 and < 123;
        }

        if (!AtChar())
            return ParseOperator();

        string symbol = "";
        while (AtChar() && NotEof)
        {
            symbol += Eat();
        }

        if (symbol == "mod")
            return new Token(TokenType.Operator, "%");

        return new Token(TokenType.Symbol, symbol);
    }

    private Token ParseOperator()
    {
        var at = At;

        if (at is '+' or '-' or '*' or '/' or '%' or '^' or '!')
        {
            if (at == '%')
            {
                //ResultLogger.LogWarning("Assuming '%' is referring to math"); TODO
            }

            return new Token(TokenType.Operator, Eat().ToString());
        }

        return ParseParen();
    }

    private Token ParseParen()
    {
        if (At == '(')
            return new Token(TokenType.OpenParen, Eat().ToString());
        if (At == ')')
            return new Token(TokenType.CloseParen, Eat().ToString());
        if (At == '|')
            return new Token(TokenType.Pipe, Eat().ToString());

        return ParseSpecial();
    }

    private Token ParseSpecial()
    {
        if (At == '_')
            return new Token(TokenType.Base, Eat().ToString());

        if (At == ',')
            return new Token(TokenType.Comma, Eat().ToString());

        return ParseWhiteSpace();
    }

    private Token ParseWhiteSpace()
    {
        if (Eat().ToString().Trim().Length == 0)
            return ParseNumber();

        return new Token(TokenType.Eof, "eof");
    }

    private bool NotEof => _text.Length != 0;

    private char At => NotEof ? _text[0] : '\0';

    private char Eat()
    {
        var at = At;

        if (NotEof)
            _text = _text[1..];

        return at;
    }
}