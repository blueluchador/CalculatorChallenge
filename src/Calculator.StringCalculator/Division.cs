namespace Calculator.StringCalculator;

public class Division : IOperation
{
    public string DisplayOperator => "/";
    
    public string Calculate(IEnumerable<long> numbers)
    {
        using var enumerator = numbers.GetEnumerator();
        // if (!enumerator.MoveNext())
        // {
        //     throw new ArgumentException("The collection must contain at least one number.");
        // }
        enumerator.MoveNext();
        
        double result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current == 0)
            {
                throw new DivideByZeroException();
            }
            result /= enumerator.Current;
        }

        return result.ToString("F4");
    }
}