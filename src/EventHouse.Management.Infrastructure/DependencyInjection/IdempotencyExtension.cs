using Core.Idempotency.DependencyInjection;
using Core.Idempotency.Options;
using Core.Idempotency.PostgreSql.DependencyInjection;
using Core.Idempotency.Redis.DependencyInjection;
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

        services.AddCoreIdempotency(
            _ => _.CopyFrom(options));

        if(options.Enabled == false)
        {
            return services;
        }

        ConfigureProvider(
            services,
            configuration,
            section);

        return services;
    }

    private static void ConfigureProvider(
        IServiceCollection services,
        IConfiguration configuration,
        IConfigurationSection section)
    {
        var provider =
            section.GetValue<string>("Provider");

        switch (provider)
        {
            case "Redis":
                services.AddCoreIdempotencyRedis(options =>
                {
                    options.Configuration =
                    configuration.CreateRedisConfiguration("MainRedis");
                });
                break;

            case "PostgreSql":
                services.AddCoreIdempotencyPostgreSql(options =>
                {
                    options.ConnectionString =
                    configuration.CreatePostgreSqlConnectionString("MainPostgreSql");
                });
                break;
        }
    }
}