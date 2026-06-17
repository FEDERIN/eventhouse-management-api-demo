using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Domain.Tests.Entities;

public class EventTests
{

    [Fact]
    public void Update_WhenNameIsNullOrWhiteSpace_ThrowsArgumentException()
    {
        // Arrange
        var validEvent = new Event(Guid.NewGuid(), "Valid Initial Name", "Initial Description");

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            validEvent.Update("", "Updated Description", EventScope.Local));

        // Assert
        Assert.Contains("Event name is required", exception.Message);
        Assert.Equal("name", exception.ParamName);
    }
}