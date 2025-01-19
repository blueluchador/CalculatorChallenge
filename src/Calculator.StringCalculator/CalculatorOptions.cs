namespace Calculator.StringCalculator;

public class CalculatorOptions
{
    public char[] DefaultDelimiters { get; init; } = [',', '\n'];
    public bool AllowNegativeNumbers { get; init; } = false;
}