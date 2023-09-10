using Helium.Lexer;
using Helium.Parser.Expressions;

namespace Helium.Parser;

public interface IParser
{
    public Expression Parse(IEnumerable<Token> tokens);
}