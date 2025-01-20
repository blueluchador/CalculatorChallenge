using System.Diagnostics;
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
            .AddTransient<ICalculatorFactory, CalculatorFactory>()
            
            // Register factory delegate for calculators
            .AddTransient<Func<string, ICalculator>>(provider =>
            {
                var factory = provider.GetService<ICalculatorFactory>();
                return operation =>
                {
                    Debug.Assert(factory != null, nameof(factory) + " != null");
                    return factory.Create(operation);
                };
            });
    }
}