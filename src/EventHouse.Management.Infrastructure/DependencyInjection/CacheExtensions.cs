using Core.Cache.DependencyInjection;
using Core.Cache.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class CacheExtensions
{
    public static IServiceCollection AddCacheInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection("Cache");

        if (!section.Exists())
            return services;

        services.AddCoreCache(options =>
        {
            section.Bind(options);

            ConfigureRedis(section, options);
        });

        return services;
    }

    private static void ConfigureRedis(
        IConfigurationSection cacheSection,
        CacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(cacheSection);
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Redis.Enabled)
            return;

        var redisSection = cacheSection.GetSection("Redis");

        var host = redisSection["Host"];

        if (string.IsNullOrWhiteSpace(host))
            throw new InvalidOperationException(
                "Cache:Redis:Host is required when Redis is enabled.");

        var password = redisSection["Password"];

        options.Redis.Configuration = redis =>
        {
            redis.EndPoints.Add(host);

            if (!string.IsNullOrWhiteSpace(password))
                redis.Password = password;
        };
    }
}