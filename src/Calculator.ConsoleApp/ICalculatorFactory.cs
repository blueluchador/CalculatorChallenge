using Calculator.StringCalculator;

namespace Calculator.ConsoleApp;

public interface ICalculatorFactory
{
    ICalculator Create(string operation);
}