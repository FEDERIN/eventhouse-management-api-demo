using Microsoft.AspNetCore.Builder;
using Core.Observability;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

public static class ObservabilityHostExtensions
{
    public static WebApplicationBuilder AddInfrastructureObservability(
        this WebApplicationBuilder builder,
        string environment,
        string serviceName,
        string serviceNamespace)
    {
        builder.AddObservability(environment, serviceName, serviceNamespace);

        return builder;
    }
}