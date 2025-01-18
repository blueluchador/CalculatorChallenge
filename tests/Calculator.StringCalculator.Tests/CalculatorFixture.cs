namespace Calculator.StringCalculator.Tests;

public class CalculatorFixture
{
    public ExpressionCalculator AdditionCalculator { get; }
    public ExpressionCalculator SubtractionCalculator { get; }
    public ExpressionCalculator MultiplicationCalculator { get; }
    public ExpressionCalculator DivisionCalculator { get; }

    public CalculatorFixture()
    {
        var options = new CalculatorOptions();
        AdditionCalculator = new ExpressionCalculator(new Addition(), options);
        SubtractionCalculator = new ExpressionCalculator(new Subtraction(), options);
        MultiplicationCalculator = new ExpressionCalculator(new Multiplication(), options);
        DivisionCalculator = new ExpressionCalculator(new Division(), options);
    }
}
