using EventHouse.Management.Infrastructure.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Infrastructure.Tests.Configuration;

public sealed class PostgreSqlConfigurationExtensionsTests
{
    [Fact]
    public void CreatePostgreSqlConnectionString_WhenSectionDoesNotExist_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .Build();

        // Act
        var act = () =>
            configuration.CreatePostgreSqlConnectionString("Main");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The configuration section 'PostgreSqlConnections:Main' was not found.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void CreatePostgreSqlConnectionString_WhenConnectionStringIsMissingOrEmpty_Throws(
        string? connectionString)
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgreSqlConnections:Main:ConnectionString"] =
                    connectionString
            })
            .Build();

        // Act
        var act = () =>
            configuration.CreatePostgreSqlConnectionString("Main");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "PostgreSqlConnections:Main:ConnectionString is required.");
    }

    [Fact]
    public void CreatePostgreSqlConnectionString_WhenConfigurationIsValid_ReturnsConnectionString()
    {
        // Arrange
        const string expected =
            "Host=localhost;Database=eventhouse;Username=test;Password=test;";

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PostgreSqlConnections:Main:ConnectionString"] =
                    expected
            })
            .Build();

        // Act
        var result =
            configuration.CreatePostgreSqlConnectionString("Main");

        // Assert
        result.Should().Be(expected);
    }
}