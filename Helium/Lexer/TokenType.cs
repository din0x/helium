namespace Helium.Lexer;

public enum TokenType
{
    Eof,

    Base,
    Comma,

    Symbol,
    Number,

    Operator,
    
    Pipe,
    OpenParen,
    CloseParen
}