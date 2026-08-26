using EventHouse.Management.Domain.Entities;
using FluentAssertions;

namespace EventHouse.Management.Domain.Tests.Entities;

public class SeatTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenSeatingRowIdIsEmpty()
    {
        // Act
        var act = () => new Seat(
            Guid.Empty,
            1,
            "1");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("seatingRowId");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNumberIsZero()
    {
        // Act
        var act = () => new Seat(
            Guid.NewGuid(),
            0,
            "1");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("number");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNumberIsNegative()
    {
        // Act
        var act = () => new Seat(
            Guid.NewGuid(),
            -1,
            "1");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("number");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenLabelIsNull()
    {
        // Act
        var act = () => new Seat(
            Guid.NewGuid(),
            1,
            null!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("label");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenLabelIsEmpty()
    {
        // Act
        var act = () => new Seat(
            Guid.NewGuid(),
            1,
            string.Empty);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("label");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenLabelIsWhitespace()
    {
        // Act
        var act = () => new Seat(
            Guid.NewGuid(),
            1,
            "   ");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("label");
    }
}