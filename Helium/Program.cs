using Helium;
using Helium.Logging;

var calculator = new Calculator();
var logger = new ConsoleLogger();

while (true)
{
    Console.Write("> ");
    var expr = Console.ReadLine() ?? "";
    if (expr == "")
        continue;
    
    var result = calculator.Calculate(expr);
    Console.Write("< ");
    logger.Log(result);
}