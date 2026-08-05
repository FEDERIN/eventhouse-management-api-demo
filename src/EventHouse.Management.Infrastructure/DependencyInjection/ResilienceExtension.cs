using Core.Resilience.Abstractions;
using Core.Resilience.DependencyInjection;
using Core.Resilience.Options;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Resilience;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class ResilienceExtensions
{
    public static IServiceCollection AddResilienceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Core:Resilience");

        if (!section.Exists())
        {
            throw new InvalidOperationException(
                "The 'Core:Resilience' configuration section is required.");
        }

        var options = new ResilienceOptions();
        section.Bind(options);

        ConfigureSqlPipeline(options);

        services.AddCoreResilience(builder => builder.CopyFrom(options));

        services.AddScoped<IApplicationResilience, ApplicationResilience>();

        return services;
    }

    private static void ConfigureSqlPipeline(ResilienceOptions options)
    {
        if (!options.ContainsPipeline(PipelineType.Sql))
        {
            throw new InvalidOperationException(
                "EventHouse requires the SQL resilience pipeline. Configure 'PipelineType.Sql' under 'Core:Resilience'.");
        }

        var pipeline = options.GetPipeline(PipelineType.Sql);

        pipeline.Retry?
            .Handle<NpgsqlException>()
            .Handle<TimeoutException>();

        pipeline.CircuitBreaker?
            .Handle<NpgsqlException>()
            .Handle<TimeoutException>();
    }
}