using Core.Cache.DependencyInjection;
using Core.Cache.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.DependencyInjection;

internal static class CacheExtensions
{
    public static IServiceCollection AddCacheInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cacheSection =
            config.GetSection("Core:Cache");

        var cacheOptions = new CacheOptions();

        cacheSection.Bind(cacheOptions);

        services.AddCoreCache(
            _ => _.CopyFrom(cacheOptions));

        //if (!cacheOptions.Enabled)
        //{
        //    return services;
        //}

        return services;
    }
}