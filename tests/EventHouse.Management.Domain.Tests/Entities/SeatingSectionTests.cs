using EventHouse.Management.Domain.Entities;
using FluentAssertions;

namespace EventHouse.Management.Domain.Tests.Entities;

public class SeatingSectionTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatingMapIdIsEmpty()
    {
        // Act
        var action = () =>
            new SeatingSection(
                Guid.Empty,
                "VIP",
                true,
                100);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("seatingMapId");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsInvalid(
        string? name)
    {
        // Act
        var action = () =>
            new SeatingSection(
                Guid.NewGuid(),
                name!,
                false,
                100);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Update_ShouldThrowArgumentException_WhenNameIsInvalid(
        string? name)
    {
        // Arrange
        var section = new SeatingSection(
            Guid.NewGuid(),
            "VIP",
            false,
            100);

        // Act
        var action = () => section.Update(name!, 100);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowArgumentException_WhenCapacityIsInvalid(
    int capacity)
    {
        // Act
        var action = () =>
            new SeatingSection(
                Guid.NewGuid(),
                "VIP",
                true,
                capacity);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(capacity));
    }
}