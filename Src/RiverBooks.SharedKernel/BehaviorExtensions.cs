using System.Reflection;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.SharedKernel;

public static class BehaviorExtensions
{
    public static IServiceCollection AddMediatRLoggingBehavior(this IServiceCollection services) =>
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    public static IServiceCollection AddMediatRFluentValidationValidationBehavior(this IServiceCollection services) =>
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));

    public static IServiceCollection AddValidatorsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        var assembly = typeof(T).GetTypeInfo().Assembly;

        var validatorTypes = assembly.GetTypes().Where(t => t.GetInterfaces().Any(IsIValidator));

        foreach (var validatorType in validatorTypes)
        {
            var implementedInterfaces = validatorType.GetInterfaces().Where(IsIValidator);

            foreach (var implementedInterface in implementedInterfaces)
            {
                services.AddTransient(implementedInterface, validatorType);
            }
        }

        return services;
    }

    private static bool IsIValidator(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>);
}