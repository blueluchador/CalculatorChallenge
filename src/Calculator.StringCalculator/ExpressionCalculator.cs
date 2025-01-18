namespace Calculator.StringCalculator;

public class ExpressionCalculator
{
    private readonly IOperation _operation;

    public ExpressionCalculator(IOperation? operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        _operation = operation;
    }

    public string Calculate(string? input)
    {
        var numbers = ParseNumbers(input).ToList();
        return $"{String.Join(_operation.DisplayOperator, numbers)} = {_operation.Calculate(numbers)}";
    }

    private static IEnumerable<long> ParseNumbers(string? input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.Count(c => c == ',') > 1)
            throw new FormatException("The expression currently supports a maximum of 2 numbers");

        return input.Split(',').Select(num => Int32.TryParse(num, out int result) ? result : 0L);
    }
}