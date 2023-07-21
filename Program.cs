using Newtonsoft.Json;

var showTokens = false;
var showAst = false;

Console.ForegroundColor = ConsoleColor.Gray;
Console.CancelKeyPress += (_, _) => {Console.ForegroundColor = ConsoleColor.Gray; };

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine() ?? "";

    if (input == "tokens")
    {
        showTokens = !showTokens;
        continue;
    }
    if (input == "ast")
    {
        showAst = !showAst;
        continue;
    }
    if (input == "clear")
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
    if (showAst)
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
    
    if (parser.Invalid)
    {
        ResultLogger.LogError("Invalid math expression");
        continue;
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