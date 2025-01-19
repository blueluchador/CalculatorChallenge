using Calculator.StringCalculator;

namespace Calculator.ConsoleApp;

public class CalculatorActions(CalculatorFactory calculatorFactory, CalculatorOptions options)
{
    public string PromptForSelection()
    {
        Console.WriteLine("Options:");
        Console.WriteLine("+ - Add (default)");
        Console.WriteLine("- - Subtract");
        Console.WriteLine("* - Multiply");
        Console.WriteLine("/ - Divide");
        Console.WriteLine("A - Set alternate delimiter");
        Console.WriteLine("N - Allow negative numbers");
        Console.WriteLine("X - Set max number");
        Console.WriteLine();
        
        Console.Write("Select an option (Ctrl+C to quit): ");
        string key = Console.ReadKey(intercept: true).KeyChar.ToString().ToLower();
    
        Console.WriteLine();

        return key;
    }
    
    public void PerformOperation(string operation)
    {
        Console.WriteLine($"{operation}: (type 'done' to calculate)");

        var lines = new List<string>();
        string line;
        while ((line = Prompt("> ")).ToLower() != "done")
        {
            lines.Add(line);
        }

        string input = String.Join('\n', lines);

        try
        {
            var calculator = calculatorFactory.Create(operation);
            string result = calculator.Calculate(input);
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void PromptForDelimiter()
    {
        string input = Prompt("> Enter alternate delimiter: ");

        if (input == "")
        {
            Console.WriteLine("Error: No delimiter provided.");
            return;
        }

        if (input.Length != 1)
        {
            Console.WriteLine("Error: The delimiter must be a single character.");
            return;
        }

        if (Char.IsDigit(input[0]))
        {
            Console.WriteLine("Error: The delimiter cannot be a number.");
            return;
        }
        
        options.DefaultDelimiters[1] = input[0];
        Console.WriteLine($"Delimiters: [{String.Join(" ", options.DefaultDelimiters)}]");
    }

    public void PromptForAllowNegatives()
    {
        string input = Prompt("> Allow negative numbers (y/n): ").ToLower();

        if (input is not ("y" or "n"))
        {
            return;
        }

        options.AllowNegativeNumbers = input == "y";
        Console.WriteLine($"Allow negative numbers: {(options.AllowNegativeNumbers ? "Yes" : "No")}");
    }

    public void PromptForMaxNumber()
    {
        string input = Prompt("> Enter the max number: ");

        if (input == "")
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

    private static string Prompt(string message = "")
    {
        Console.Write(message);
        return Console.ReadLine()?.Trim() ?? String.Empty;
    }
}