namespace Calculator.StringCalculator;

public class Addition : IOperation
{
    public string DisplayOperator => "+";

    public string Calculate(IEnumerable<long> numbers)
    {
        return numbers.Sum().ToString();
    }
}