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
        
        int newlineIndex = input.IndexOf('\n');
        if (newlineIndex == -1 || newlineIndex + 1 >= input.Length)
            throw new FormatException($"Calculate expression is missing in input: {input}");

        string delimitersPart = input[2..newlineIndex];
        string expressionPart = input[(newlineIndex + 1)..];
        
        return input switch
        {
            _ when CustomCharDelimiterRegex().Match(input) is { Success: true } customCharDelimiterMatch =>
            (
                [.._options.DefaultDelimiters, customCharDelimiterMatch.Groups[1].Value],
                customCharDelimiterMatch.Groups[2].Value
            ),
            _ when CustomDelimiterListRegex().Match(delimitersPart) is { Success: true } =>
            (
                GetDelimiterList(delimitersPart, _options.DefaultDelimiters).ToArray(),
                expressionPart
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
    
    private static IEnumerable<string> GetDelimiterList(string delimitersPart, string[] delimiters)
    {
        foreach (string d in delimiters)
        {
            yield return d; 
        }
        
        var matches = DelimiterListRegex().Matches(delimitersPart);
        foreach (Match match in matches)
        {
            string delimiter = match.Groups[1].Value;
            if (delimiter == String.Empty) throw new FormatException("Delimiters cannot be empty.");
            yield return delimiter;
        }
    }

    [GeneratedRegex(@"^//(.)\n(.+)")]
    private static partial Regex CustomCharDelimiterRegex();
    
    [GeneratedRegex(@"^(\[[^\[\]]*\])+$")]
    private static partial Regex CustomDelimiterListRegex();
    
    [GeneratedRegex(@"\[(.*?)\]")]
    private static partial Regex DelimiterListRegex();
}