using Calculator.StringCalculator;

namespace Calculator.ConsoleApp;

public class Actions(CalculatorFactory calculatorFactory, CalculatorOptions options)
{
    public void PerformOperation(string option)
    {
        string operation = option switch
        {
            "a" => "Add",
            "s" => "Subtract",
            "m" => "Multiply",
            "d" => "Divide",
            _ => "Add"
        };
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
}