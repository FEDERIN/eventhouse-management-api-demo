using Core.Idempotency.Abstractions;
using Core.Idempotency.DependencyInjection;
using Core.Idempotency.Options;
using EventHouse.Management.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class IdempotencyExtension
{
    public static IServiceCollection AddIdempotencyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Core:Idempotency");

        if (!section.Exists())
        {
            return services;
        }

        var options = new IdempotencyOptions();
        section.Bind(options);

        section.ReplaceIfConfigured(
            "AllowedMethods",
            options.AllowedMethods,
            options.AddAllowedMethods);

        section.ReplaceIfConfigured(
            "CacheableStatusCodes",
            options.CacheableStatusCodes,
            options.AddCacheableStatusCodes);

        if (options.Provider == IdempotencyProviderType.Redis)
        {
            options.Redis.Configuration =
                configuration.CreateRedisConfiguration("MainRedis");
        }

        services.AddCoreIdempotency(_ => _.CopyFrom(options));

        return services;
    }
}