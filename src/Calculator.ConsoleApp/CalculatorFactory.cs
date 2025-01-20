using Calculator.StringCalculator;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ConsoleApp;

public class CalculatorFactory(IServiceProvider provider) : ICalculatorFactory
{
    public ICalculator Create(string operation)
    {
        var options = provider.GetRequiredService<CalculatorOptions>();
        
        return operation switch
        {
            Operations.Add => new ExpressionCalculator(provider.GetService<Addition>(), options),
            Operations.Subtract => new ExpressionCalculator(provider.GetService<Subtraction>(), options),
            Operations.Multiply => new ExpressionCalculator(provider.GetService<Multiplication>(), options),
            Operations.Divide => new ExpressionCalculator(provider.GetService<Division>(), options),
            _ => new ExpressionCalculator(provider.GetService<Addition>(), options),
        };
    }
}