namespace Helium.Lexer;

public interface ILexer
{
    public IEnumerable<Token> Tokenize(string text);
}