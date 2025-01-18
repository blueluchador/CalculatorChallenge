namespace Calculator.StringCalculator;

public interface IOperation
{
    string DisplayOperator { get; }
    
    string Calculate(IEnumerable<long> numbers);
}