using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Infrastructure.Configuration;

internal static class ConfigurationCollectionExtensions
{
    public static void ReplaceIfConfigured(
        this IConfigurationSection section,
        string key,
        HashSet<string> target,
        Action<string[]> addAction)
    {
        var values = section.GetSection(key).Get<string[]>();

        if (values is null)
            return;

        target.Clear();
        addAction(values);
    }

    public static void ReplaceIfConfigured(
        this IConfigurationSection section,
        string key,
        HashSet<int> target,
        Action<int[]> addAction)
    {
        var values = section.GetSection(key).Get<int[]>();

        if (values is null)
            return;

        target.Clear();
        addAction(values);
    }
}