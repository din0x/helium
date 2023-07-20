using Calculator;
using Calculator.Results;
using Newtonsoft.Json;

var showTokens = false;
var showAST = false;

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine() ?? "";

    if (input == "tokens")
    {
        showTokens = !showTokens;
        continue;
    }
    else if (input == "ast")
    {
        showAST = !showAST;
        continue;
    }
    else if (input == "clear")
    {
        Console.Clear();
        continue;
    }

    var lexer = new Lexer(input);


    if (showTokens)
    {
        Console.WriteLine();
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token);
        }
    }

    var parser = new Parser(lexer.Tokens);
    if (parser.Invalid)
    {
        ResultLogger.LogError("Invalid math expression.");
        continue;
    }

    if (showAST)
    {
        var settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            CheckAdditionalContent = true,
        };
        Console.WriteLine();
        Console.WriteLine(JsonConvert.SerializeObject(parser.Expr, settings));
        Console.WriteLine();
    }

    try
    {
        var result = parser.Expr.Evaluate();
        ResultLogger.Log(result);
    } 
    catch (NotImplementedException)
    {

    }
}