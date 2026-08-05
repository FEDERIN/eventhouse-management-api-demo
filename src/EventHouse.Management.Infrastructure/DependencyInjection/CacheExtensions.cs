using Core.Cache.Abstractions;
using Core.Cache.DependencyInjection;
using Core.Cache.Options;
using EventHouse.Management.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class CacheExtensions
{
    public static IServiceCollection AddCacheInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var distributedCache = config.GetSection("Core:Cache");

        if (!distributedCache.Exists())
            return services;

        services.AddCoreCache(options =>
        {
            distributedCache.Bind(options);

            if (options.DefaultProvider == CacheProviderType.Redis)
            {
                options.Redis.Configuration = 
                config.CreateRedisConfiguration("MainRedis");
            }
        });


        return services;
    }
}