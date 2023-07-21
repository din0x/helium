namespace Calculator;

public class Lexer
{
    public Token[] Tokens;

    public Lexer(string text)
    {
        Tokens = Array.Empty<Token>();
        _text = text;
        Tokenize();
    }

    private string _text;

    private void Tokenize()
    {
        var tokens = new List<Token>();
        while (NotEof)
        {
            var tok = Number();
            if (tok.Type != TokenType.Empty)
                tokens.Add(tok);
        }
        Tokens = tokens.ToArray();
    }

    private Token Number()
    {
        bool AtNumber()
        {
            var atbyte = (byte)At();
            return atbyte is > 47 and < 58;
        }

        if (!AtNumber()) 
            return Symbol();

        string number = "";
        var usedDot = false;
        while ((AtNumber() || (!usedDot && At() == '.')) && NotEof)
        {
            if (At() == '.')
                usedDot = true;
            
            number += Eat();
        }

        return new Token(TokenType.Number, number.Replace('.', ','));
    }

    private Token Symbol()
    {
        bool AtChar()
        {
            var atbyte = (byte)At();
            return (atbyte > 64 && atbyte < 91) || (atbyte > 96 && atbyte < 123);
        }

        if (!AtChar())
            return Operator();

        string symbol = "";
        while (AtChar() && NotEof)
        {
            symbol += Eat();
        }

        if (symbol == "mod")
            return new Token(TokenType.Operator, symbol);

        return new Token(TokenType.Symbol, symbol);
    }

    private Token Operator()
    {
        var at = At();

        if (at is '+' or '-' or '*' or '/' or '%' or '^' or '!')
        {
            if (at == '%')
            {
                ResultLogger.LogWarning("Assuming '%' is referring to math");
            }

            return new Token(TokenType.Operator, Eat().ToString());
        }

        return Paren();
    }

    private Token Paren()
    {
        if (At() == '(')
            return new Token(TokenType.OpenParen, Eat().ToString());
        if (At() == ')')
            return new Token(TokenType.CloseParen, Eat().ToString());
        if (At() == '|')
            return new Token(TokenType.Pipe, Eat().ToString());

        return Base();
    }

    private Token Base()
    {
        if (At() == '_')
            return new Token(TokenType.Base, Eat().ToString());

        return Comma();
    }

    private Token Comma()
    {
        if (At() == ',')
            return new Token(TokenType.Comma, Eat().ToString());

        return WhiteSpace();
    }

    private Token WhiteSpace()
    {
        if (Eat().ToString().Trim().Length == 0)
            return new Token(TokenType.Empty, "");

        return new Token(TokenType.None, "");
    }

    private bool NotEof => _text.Length != 0;

    private char At()
    {
        if (NotEof)
            return _text[0];

        return '\0';
    }

    private char Eat()
    {
        var at = At();

        if (NotEof)
            _text = _text[1..];

        return at;
    }
}
