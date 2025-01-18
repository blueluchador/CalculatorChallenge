namespace Calculator.StringCalculator.Tests;

public class CalculatorFixture
{
    public ExpressionCalculator AdditionCalculator { get; } = new(new Addition());
    public ExpressionCalculator SubtractionCalculator { get; } = new(new Subtraction());
    public ExpressionCalculator MultiplicationCalculator { get; } = new(new Multiplication());
    public ExpressionCalculator DivisionCalculator { get; } = new(new Division());
}
