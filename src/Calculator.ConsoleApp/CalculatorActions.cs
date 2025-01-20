using Calculator.StringCalculator;

namespace Calculator.ConsoleApp;

public class CalculatorActions(ICalculatorFactory calculatorFactory, CalculatorOptions options, ICustomConsole console)
{
    public string PromptForSelection()
    {
        console.WriteLine("Options:");
        console.WriteLine("+ - Add (default)");
        console.WriteLine("- - Subtract");
        console.WriteLine("* - Multiply");
        console.WriteLine("/ - Divide");
        console.WriteLine("A - Set alternate delimiter");
        console.WriteLine("N - Allow negative numbers");
        console.WriteLine("X - Set max number");
        console.WriteLine();
        
        string input = console.PromptKey("Select an option (Ctrl+C to quit): ").ToString().ToLower();
        console.WriteLine();

        return input;
    }
    
    public void PerformOperation(string operation)
    {
        console.WriteLine($"{operation}: (type 'done' to calculate)");

        var lines = new List<string>();
        string line;
        while (!(line = console.Prompt("> ")).Equals("done", StringComparison.CurrentCultureIgnoreCase))
        {
            lines.Add(line);
        }

        string input = String.Join('\n', lines);

        try
        {
            var calculator = calculatorFactory.Create(operation);
            string result = calculator.Calculate(input);
            console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void PromptForDelimiter()
    {
        string input = console.Prompt("> Enter alternate delimiter: ");

        if (input == "")
        {
            console.WriteLine("Error: No delimiter provided.");
            return;
        }

        if (input.Length != 1)
        {
            console.WriteLine("Error: The delimiter must be a single character.");
            return;
        }

        if (Char.IsDigit(input[0]))
        {
            console.WriteLine("Error: The delimiter cannot be a number.");
            return;
        }
        
        options.DefaultDelimiters[1] = input;
        console.WriteLine($"Delimiters: [{String.Join(" ", options.DefaultDelimiters)}]");
    }

    public void PromptForAllowNegatives()
    {
        string input = console.Prompt("> Allow negative numbers (y/n): ").ToLower();

        if (input is not ("y" or "n"))
        {
            return;
        }

        options.AllowNegativeNumbers = input == "y";
        console.WriteLine($"Allow negative numbers: {(options.AllowNegativeNumbers ? "Yes" : "No")}");
    }

    public void PromptForMaxNumber()
    {
        string input = console.Prompt("> Enter the max number: ");

        if (input == "")
        {
            console.WriteLine("Error: No max number provided.");
            return;
        }

        if (!Int32.TryParse(input, out int maxNumber))
        {
            console.WriteLine("Error: The max number is not valid.");
            return;
        }

        if (maxNumber < 1)
        {
            console.WriteLine("Error: The max number cannot be less than 1.");
            return;
        }
        
        options.MaxNumber = maxNumber;
        console.WriteLine($"Max number: {options.MaxNumber}");
    }
}