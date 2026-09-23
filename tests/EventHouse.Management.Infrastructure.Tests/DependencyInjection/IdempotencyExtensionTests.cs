using EventHouse.Management.Infrastructure.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHouse.Management.Infrastructure.Tests.DependencyInjection;

public sealed class IdempotencyExtensionTests
{
    [Theory]
    [InlineData("SqlServer")]
    [InlineData("")]
    public void AddIdempotencyInfrastructure_WhenProviderIsUnsupported_Throws(string provider)
    {
        var configuration = CreateConfiguration(provider);
        var services = new ServiceCollection();

        var act = () => services.AddIdempotencyInfrastructure(configuration);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Unsupported idempotency provider: {provider}");
    }

    [Fact]
    public void AddIdempotencyInfrastructure_WhenPostgreSqlProviderUsesDifferentCasing_DoesNotThrow()
    {
        var configuration = CreateConfiguration("PostgreSQL");
        var services = new ServiceCollection();

        var act = () => services.AddIdempotencyInfrastructure(configuration);

        act.Should().NotThrow();
    }

    private static IConfiguration CreateConfiguration(string provider)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Core:Idempotency:Enabled"] = "true",
                ["Core:Idempotency:Provider"] = provider,
                ["PostgreSqlConnections:MainPostgreSql:ConnectionString"] =
                    "Host=localhost;Database=idempotency;Username=test;Password=test"
            })
            .Build();
    }
}
