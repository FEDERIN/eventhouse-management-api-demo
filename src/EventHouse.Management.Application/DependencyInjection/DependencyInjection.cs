using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using EventHouse.Management.Application.Common.Behaviors;

namespace EventHouse.Management.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly));

        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
