using Calculator.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

// Handles Ctrl+C key press
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

var calcActions = serviceProvider.GetService<CalculatorActions>();
if (calcActions == null) throw new Exception("Calculator actions not found!");

// Run the string calculator
while (true)
{
    string selection = calcActions.PromptForSelection();

    // Perform the requested action
    Action action = selection switch
    {
        "+" or "=" => () => calcActions.PerformOperation(Operations.Add),
        "-" => () => calcActions.PerformOperation(Operations.Subtract),
        "*" or "8" => () => calcActions.PerformOperation(Operations.Multiply),
        "/" => () => calcActions.PerformOperation(Operations.Divide),
        "a" => () => calcActions.PromptForDelimiter(),
        "n" => () => calcActions.PromptForAllowNegatives(),
        "x" => () => calcActions.PromptForMaxNumber(),
        _ => () => calcActions.PerformOperation(Operations.Add),
    };
    action();
    
    Console.WriteLine();
}