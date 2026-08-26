using EventHouse.Management.Infrastructure.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace EventHouse.Management.Infrastructure.Tests.Configuration;

public sealed class RedisConfigurationExtensionsTests
{
    [Fact]
    public void CreateRedisConfiguration_WhenSectionDoesNotExist_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .Build();

        // Act
        var act = () =>
            configuration.CreateRedisConfiguration("Main");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The configuration section 'RedisConnections:Main' was not found.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void CreateRedisConfiguration_WhenHostIsMissingOrEmpty_Throws(
        string? host)
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RedisConnections:Main:Host"] = host,
                ["RedisConnections:Main:Password"] = "password"
            })
            .Build();

        // Act
        var act = () =>
            configuration.CreateRedisConfiguration("Main");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "RedisConnections:Main:Host is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void CreateRedisConfiguration_WhenPasswordIsMissingOrEmpty_Throws(
        string? password)
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RedisConnections:Main:Host"] = "localhost:6379",
                ["RedisConnections:Main:Password"] = password
            })
            .Build();

        // Act
        var act = () =>
            configuration.CreateRedisConfiguration("Main");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "RedisConnections:Main:Password is required.");
    }

    [Fact]
    public void CreateRedisConfiguration_WhenConfigurationIsValid_ReturnsConfigurationAction()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RedisConnections:Main:Host"] = "localhost:6379",
                ["RedisConnections:Main:Password"] = "password"
            })
            .Build();

        // Act
        var result =
            configuration.CreateRedisConfiguration("Main");

        // Assert
        result.Should().NotBeNull();
    }
}