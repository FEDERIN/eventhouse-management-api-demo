using Core.Idempotency;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Errors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDatabaseInfrastructure(config);
        services.AddIdempotencyProvider(config);
        services.AddRepositories();
        services.AddSingleton<IExceptionMapper, ExceptionMapper>();

        return services;
    }
}