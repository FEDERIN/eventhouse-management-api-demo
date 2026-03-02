using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Queries.Events.GetAll;


namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class EventSortMapperTests
{
    [Theory]
    [InlineData(EventSortBy.Name, EventSortField.Name)]
    [InlineData(EventSortBy.Description, EventSortField.Description)]
    [InlineData(EventSortBy.Scope, EventSortField.Scope)]
    public void ToApplication_WhenSortByIsValid_ReturnsMappedField(
        EventSortBy input,
        EventSortField expected)
    {
        // Act
        var result = EventSortMapper.ToApplication(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplication_WhenSortByIsNull_ReturnsNull()
    {
        // Act
        var result = EventSortMapper.ToApplication(null);

        // Assert
        Assert.Null(result);
    }
}
