namespace Calculator.StringCalculator;

public class Multiplication : IOperation
{
    public string DisplayOperator => "*";
    
    public string Calculate(IEnumerable<long> numbers)
    {
        return numbers.Aggregate((result, next) => result * next).ToString();
    }
}