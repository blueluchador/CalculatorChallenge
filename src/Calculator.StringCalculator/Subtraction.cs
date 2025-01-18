namespace Calculator.StringCalculator;

public class Subtraction : IOperation
{
    public string DisplayOperator => "-";
    
    public string Calculate(IEnumerable<long> numbers)
    {
        return numbers.Aggregate((result, next) => result - next).ToString();
    }
}