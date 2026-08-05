using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EventHouse.Management.Infrastructure.Configuration;

internal static class RedisConfigurationExtensions
{
    public static Action<ConfigurationOptions> CreateRedisConfiguration(
        this IConfiguration configuration,
        string connectionName)
    {
        var section = configuration.GetSection($"RedisConnections:{connectionName}");

        Validate(section, connectionName);

        return options =>
        {
            options.EndPoints.Add(section["Host"]!);
            options.Password = section["Password"];

            if (int.TryParse(section["Database"], out var database))
            {
                options.DefaultDatabase = database;
            }

            if (bool.TryParse(section["Ssl"], out var ssl))
            {
                options.Ssl = ssl;
            }
        };
    }

    private static void Validate(
        IConfigurationSection section,
        string connectionName)
    {
        if (!section.Exists())
        {
            throw new InvalidOperationException(
                $"The configuration section 'RedisConnections:{connectionName}' was not found.");
        }

        if (string.IsNullOrWhiteSpace(section["Host"]))
        {
            throw new InvalidOperationException(
                $"RedisConnections:{connectionName}:Host is required.");
        }

        if (string.IsNullOrWhiteSpace(section["Password"]))
        {
            throw new InvalidOperationException(
                $"RedisConnections:{connectionName}:Password is required.");
        }
    }
}