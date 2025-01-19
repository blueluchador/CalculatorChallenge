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
    .AddTransient<Actions>()
    .BuildServiceProvider();

Console.Clear();

Console.WriteLine("Options:");
Console.WriteLine("A - Add (default)");
Console.WriteLine("S - Subtract");
Console.WriteLine("M - Multiply");
Console.WriteLine("D - Divide");
Console.WriteLine("C - Change alternate delimiter");
Console.WriteLine();

var actions = serviceProvider.GetService<Actions>();

// Run the string calculator
while (true)
{
    Console.Write("Select an option (Ctrl+C to quit): ");
    string key = Console.ReadKey().KeyChar.ToString().ToLower();
    
    Console.WriteLine();

    // Perform the requested action
    switch (key)
    {
        case "a":
        case "s":
        case "m":
        case "d":
            actions!.PerformOperation(key); break;
        case "c": actions!.PromptForDelimiter(); break;
        default: actions!.PerformOperation("a"); break;
    }
    
    Console.WriteLine();
}