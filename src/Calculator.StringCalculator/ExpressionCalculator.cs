namespace Calculator.StringCalculator;

public class ExpressionCalculator
{
    private readonly IOperation _operation;
    private readonly CalculatorOptions _options;

    public ExpressionCalculator(IOperation? operation, CalculatorOptions options)
    {
        ArgumentNullException.ThrowIfNull(operation);
        
        _operation = operation;
        _options = options;
        
        if (_options.DefaultDelimiters.Any(Char.IsDigit))
            throw new ArgumentException("Delimiters cannot be a number.");
        
        if (_options.MaxNumber < 1)
            throw new ArgumentException("Max number cannot be less than 1.");
    }

    public string Calculate(string? input)
    {
        var numbers = ParseNumbers(input).ToList();
        return $"{String.Join(_operation.DisplayOperator, numbers)} = {_operation.Calculate(numbers)}";
    }

    private List<long> ParseNumbers(string? input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var numbers = input.Split(_options.DefaultDelimiters).Select(n =>
        {
            long num = Int32.TryParse(n, out int result) ? result : 0L;
            return num > _options.MaxNumber ? 0 : num;
        }).ToList();
        
        ValidateNumbers(numbers);

        return numbers;
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
}