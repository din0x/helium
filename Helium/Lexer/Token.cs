namespace Helium.Lexer;

public sealed class Token
{
    public TokenType Type;
    public string Value;

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type,-10} {Value}";
    }
}