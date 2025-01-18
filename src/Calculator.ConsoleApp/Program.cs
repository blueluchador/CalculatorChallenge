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
    .BuildServiceProvider();

// Resolve the calculator factory
var calculatorFactory = serviceProvider.GetService<Func<string?, ExpressionCalculator>>();

// Run the string calculator
while (true)
{
    Console.WriteLine("Enter a list of numbers separated by commas:");
    string? input = Console.ReadLine();
    
    Console.Write("Choose an operation: (A)dd (default), (S)ubtract, (M)ultiply, or (D)ivide: ");
    string? operation = Console.ReadLine()?.Trim().ToLower();
    
    try
    {
        var calc = calculatorFactory!(operation);
        string result = calc.Calculate(input);
        Console.WriteLine($"Result: {result}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}