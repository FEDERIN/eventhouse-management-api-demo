using System.Reflection;

namespace EventHouse.Management.Architecture.Tests;

public sealed class ArchitectureDependencyTests
{
    [Fact]
    public void ShareKernel_should_not_depend_on_management_layers()
    {
        AssertDoesNotReference(
            "EventHouse.ShareKernel",
            "EventHouse.Management.Domain",
            "EventHouse.Management.Application",
            "EventHouse.Management.Infrastructure",
            "EventHouse.Management.Api");
    }

    [Fact]
    public void Domain_should_not_depend_on_outer_layers_or_technical_frameworks()
    {
        AssertDoesNotReference(
            "EventHouse.Management.Domain",
            "EventHouse.Management.Application",
            "EventHouse.Management.Infrastructure",
            "EventHouse.Management.Api");

        AssertDoesNotReferenceAssembliesWithPrefix(
            "EventHouse.Management.Domain",
            "CoreSystem.",
            "Microsoft.AspNetCore.",
            "Microsoft.EntityFrameworkCore",
            "Npgsql");
    }

    [Fact]
    public void Application_should_not_depend_on_outer_layers_or_technical_frameworks()
    {
        AssertDoesNotReference(
            "EventHouse.Management.Application",
            "EventHouse.Management.Infrastructure",
            "EventHouse.Management.Api");

        AssertDoesNotReferenceAssembliesWithPrefix(
            "EventHouse.Management.Application",
            "CoreSystem.",
            "Microsoft.AspNetCore.",
            "Microsoft.EntityFrameworkCore",
            "Npgsql");
    }

    [Fact]
    public void Infrastructure_should_not_depend_on_api()
    {
        AssertDoesNotReference(
            "EventHouse.Management.Infrastructure",
            "EventHouse.Management.Api");
    }

    private static void AssertDoesNotReference(string assemblyName, params string[] forbiddenAssemblyNames)
    {
        var references = GetReferences(assemblyName);

        foreach (var forbiddenAssemblyName in forbiddenAssemblyNames)
        {
            Assert.DoesNotContain(forbiddenAssemblyName, references);
        }
    }

    private static void AssertDoesNotReferenceAssembliesWithPrefix(
        string assemblyName,
        params string[] forbiddenPrefixes)
    {
        var references = GetReferences(assemblyName);

        foreach (var forbiddenPrefix in forbiddenPrefixes)
        {
            Assert.DoesNotContain(references, reference => reference.StartsWith(forbiddenPrefix, StringComparison.Ordinal));
        }
    }

    private static HashSet<string> GetReferences(string assemblyName) =>
        Assembly.Load(assemblyName)
            .GetReferencedAssemblies()
            .Select(reference => reference.Name!)
            .ToHashSet(StringComparer.Ordinal);
}
