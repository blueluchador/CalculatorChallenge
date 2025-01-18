using Calculator.StringCalculator;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ConsoleApp;

public class CalculatorFactory(IServiceProvider provider)
{
    public ExpressionCalculator Create(string operation)
    {
        var options = provider.GetRequiredService<CalculatorOptions>();
        
        return operation switch
        {
            "Add" => new ExpressionCalculator(provider.GetService<Addition>(), options),
            "Subtract" => new ExpressionCalculator(provider.GetService<Subtraction>(), options),
            "Multiply" => new ExpressionCalculator(provider.GetService<Multiplication>(), options),
            "Divide" => new ExpressionCalculator(provider.GetService<Division>(), options),
            _ => new ExpressionCalculator(provider.GetService<Addition>(), options),
        };
    }
}