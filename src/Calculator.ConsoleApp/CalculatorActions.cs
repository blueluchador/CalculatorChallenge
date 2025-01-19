using Calculator.StringCalculator;

namespace Calculator.ConsoleApp;

public class CalculatorActions(CalculatorFactory calculatorFactory, CalculatorOptions options)
{
    public void PerformOperation(string operation)
    {
        Console.WriteLine($"{operation}: (type 'done' to calculate)");
    
        string input = "";
        Console.Write("> ");
        string? line = Console.ReadLine();
        while (line?.Trim().ToLower() != "done")
        {
            input += line + '\n';
            Console.Write("> ");
            line = Console.ReadLine();
        }
        input = input.TrimEnd('\n');

        try
        {
            var calc = calculatorFactory.Create(operation);
            string result = calc.Calculate(input);
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void PromptForDelimiter()
    {
        Console.Write("> Enter alternate delimiter: ");
        string? delimiter = Console.ReadLine()?.Trim();

        if (delimiter is "" or null)
        {
            Console.WriteLine("Error: No delimiter provided.");
            return;
        }

        if (delimiter.Length != 1)
        {
            Console.WriteLine("Error: The delimiter must be one character.");
            return;
        }

        if (Char.IsDigit(delimiter[0]))
        {
            Console.WriteLine("Error: The delimiter cannot be a number.");
            return;
        }
        
        options.DefaultDelimiters[1] = delimiter[0];
        Console.WriteLine($"Delimiters: [{String.Join(" ", options.DefaultDelimiters)}]");
    }

    public void PromptForAllowNegatives()
    {
        Console.Write("> Allow negative numbers (y/n): ");
        string? input = Console.ReadLine()?.Trim().ToLower();

        if (input is not ("y" or "n"))
        {
            return;
        }

        options.AllowNegativeNumbers = input == "y";
        Console.WriteLine($"Allow negative numbers: {(options.AllowNegativeNumbers ? "Yes" : "No")}");
    }

    public void PromptForMaxNumber()
    {
        Console.Write("> Enter the max number: ");
        string? input = Console.ReadLine()?.Trim();

        if (input is "" or null)
        {
            Console.WriteLine("Error: No max number provided.");
            return;
        }

        if (!Int32.TryParse(input, out int maxNumber))
        {
            Console.WriteLine("Error: The max number is not valid.");
            return;
        }

        if (maxNumber < 1)
        {
            Console.WriteLine("Error: The max number cannot be less than 1.");
        }
        
        options.MaxNumber = maxNumber;
        Console.WriteLine($"Max number: {options.MaxNumber}");
    }
}