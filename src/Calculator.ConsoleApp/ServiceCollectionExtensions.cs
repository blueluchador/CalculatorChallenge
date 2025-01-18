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
            
            // Register calculator options
            .AddSingleton<CalculatorOptions>()
            
            // Register the CalculatorFactory
            .AddTransient<CalculatorFactory>()
            
            // Register factory delegate for calculators
            .AddTransient<Func<string, ExpressionCalculator>>(provider =>
            {
                var factory = provider.GetService<CalculatorFactory>();
                return operation => factory!.Create(operation);
            });
    }
}