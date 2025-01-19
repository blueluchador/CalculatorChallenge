using Calculator.ConsoleApp;
using Calculator.StringCalculator;
using Microsoft.Extensions.DependencyInjection;

Console.CancelKeyPress += (_, _) =>
{
    Console.WriteLine("\nGoodbye!");
};

// Configure the calculator services
var serviceProvider = new ServiceCollection()
    .AddStringCalculatorServices()
    .AddTransient<CalculatorActions>()
    .BuildServiceProvider();

Console.Clear();

Console.WriteLine("Options:");
Console.WriteLine("+ - Add (default)");
Console.WriteLine("- - Subtract");
Console.WriteLine("* - Multiply");
Console.WriteLine("/ - Divide");
Console.WriteLine("A - Set alternate delimiter");
Console.WriteLine("N - Allow negative numbers");
Console.WriteLine("X - Set max number");
Console.WriteLine();

var calcActions = serviceProvider.GetService<CalculatorActions>();
if (calcActions == null) throw new Exception("Calculator actions not found!");

// Run the string calculator
while (true)
{
    Console.Write("Select an option (Ctrl+C to quit): ");
    string key = Console.ReadKey(intercept: true).KeyChar.ToString().ToLower();
    
    Console.WriteLine();

    // Perform the requested action
    Action action = key switch
    {
        "+" or "=" => () => calcActions.PerformOperation(Operations.Add),
        "-" => () => calcActions.PerformOperation(Operations.Subtract),
        "*" or "8" => () => calcActions.PerformOperation(Operations.Multiply),
        "/" => () => calcActions.PerformOperation(Operations.Divide),
        "a" => () => calcActions.PromptForDelimiter(),
        "n" => () => calcActions.PromptForAllowNegatives(),
        "x" => () => calcActions.PromptForMaxNumber(),
        _ => () => calcActions.PerformOperation("Add"),
    };
    action();
    
    Console.WriteLine();
}