using System.Text.RegularExpressions;

namespace Calculator.StringCalculator;

public partial class ExpressionCalculator
{
    private readonly IOperation _operation;
    private readonly CalculatorOptions _options;

    public ExpressionCalculator(IOperation? operation, CalculatorOptions options)
    {
        ArgumentNullException.ThrowIfNull(operation);
        
        _operation = operation;
        _options = options;
        
        if (_options.DefaultDelimiters.Any(d => Char.IsDigit(d[0])))
            throw new ArgumentException("Delimiters cannot be a number.");
        
        if (_options.MaxNumber < 1)
            throw new ArgumentException("Max number cannot be less than 1.");
    }

    public string Calculate(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        
        (string[] delimiters, string numbers) = ParseInput(input);
        var parsedNumbers = ParseNumbers(numbers, delimiters);
        return $"{String.Join(_operation.DisplayOperator, parsedNumbers)} = {_operation.Calculate(parsedNumbers)}";
    }

    private List<long> ParseNumbers(string input, string[] delimiters)
    {
        ArgumentNullException.ThrowIfNull(input);

        var numbers = input.Split(delimiters, StringSplitOptions.None).Select(n =>
        {
            long num = Int32.TryParse(n, out int result) ? result : 0L;
            return num > _options.MaxNumber ? 0 : num;
        }).ToList();
        
        ValidateNumbers(numbers);

        return numbers;
    }
    
    private (string[] delimiters, string numbers) ParseInput(string input)
    {
        string numbers = input;
        if (!input.StartsWith("//")) return (_options.DefaultDelimiters, numbers);
        
        return input switch
        {
            _ when CharDelimiterRegex().Match(input) is { Success: true } charDelimiterMatch =>
            (
                [.._options.DefaultDelimiters, charDelimiterMatch.Groups[1].Value],
                charDelimiterMatch.Groups[2].Value
            ),
            _ when StringDelimiterRegex().Match(input) is { Success: true } stringDelimiterMatch =>
            (
                [.._options.DefaultDelimiters, stringDelimiterMatch.Groups[1].Value],
                stringDelimiterMatch.Groups[2].Value
            ),
            _ => throw new FormatException($"Custom delimiter format is invalid or missing expression: {input}")
        };
    }
    
    private void ValidateNumbers(IEnumerable<long> numbers)
    {
        if (_options.AllowNegativeNumbers) return;
        
        var negatives = numbers.Where(num => num < 0);
        long[] negativesArray = negatives as long[] ?? negatives.ToArray();
        if (negativesArray.Length != 0)
        {
            throw new FormatException($"Negative numbers are not allowed: {String.Join(',', negativesArray)}");
        }
    }

    [GeneratedRegex(@"^//(.)\n(.+)")]
    private static partial Regex CharDelimiterRegex();
    
    [GeneratedRegex(@"//\[(.*?)\]\n(.+)")]
    private static partial Regex StringDelimiterRegex();
}