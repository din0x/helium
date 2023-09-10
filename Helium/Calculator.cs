using Helium.Lexer;
using Helium.Parser;
using Newtonsoft.Json;

namespace Helium;

public sealed class Calculator
{
    private readonly ILexer _lexer;
    private readonly IParser _parser;
    
    public Calculator(ILexer? lexer = null, IParser? parser = null)
    {
        _lexer = lexer ?? new Lexer.Lexer();
        _parser = parser ?? new Parser.Parser();
    }
    
    public Result Calculate(string exprsession)
    {
        var tokens = new List<Token>(_lexer.Tokenize(exprsession));
        /*foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }*/
        
        var expr = _parser.Parse(tokens);

        //Console.WriteLine(JsonConvert.SerializeObject(expr, Formatting.Indented));
        
        var result = new Result(expr.Evaluate());
        
        return result;
    }
}