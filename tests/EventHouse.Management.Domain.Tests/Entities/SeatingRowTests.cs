using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class SeatingRowTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatingSectionIdIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.Empty,
                1,
                "A"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNumberIsZero()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.NewGuid(),
                0,
                "A"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNumberIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.NewGuid(),
                -1,
                "A"));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenLabelIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.NewGuid(),
                1,
                null!));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenLabelIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.NewGuid(),
                1,
                string.Empty));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenLabelIsWhitespace()
    {
        Assert.Throws<ArgumentException>(() =>
            new SeatingRow(
                Guid.NewGuid(),
                1,
                "   "));
    }
}