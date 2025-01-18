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
    }

    public string Calculate(string? input)
    {
        var numbers = ParseNumbers(input).ToList();
        return $"{String.Join(_operation.DisplayOperator, numbers)} = {_operation.Calculate(numbers)}";
    }

    private IEnumerable<long> ParseNumbers(string? input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return input.Split(_options.DefaultDelimiters)
            .Select(num => Int32.TryParse(num, out int result) ? result : 0L);
    }
}