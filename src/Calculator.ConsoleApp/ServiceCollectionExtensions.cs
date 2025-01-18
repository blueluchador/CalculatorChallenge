using Calculator.StringCalculator;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ConsoleApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStringCalculatorServices(this IServiceCollection services)
    {
        return services
            // Register operations
            .AddTransient<Addition>()
            .AddTransient<Subtraction>()
            .AddTransient<Multiplication>()
            .AddTransient<Division>()
            
            // Register factory for calculators
            .AddTransient<Func<string, ExpressionCalculator>>(provider => operation =>
            {
                return operation switch
                {
                    "add" or "a" => new ExpressionCalculator(provider.GetService<Addition>()),
                    "subtract" or "s" => new ExpressionCalculator(provider.GetService<Subtraction>()),
                    "multiply" or "m" => new ExpressionCalculator(provider.GetService<Multiplication>()),
                    "divide" or "d" => new ExpressionCalculator(provider.GetService<Division>()),
                    _ => new ExpressionCalculator(provider.GetService<Addition>()),
                };
            });
    }
}