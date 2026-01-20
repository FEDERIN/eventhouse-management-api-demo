using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Genres;
using EventHouse.Management.Application.Queries.Genres.GetAll;


namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class GenreSortMapperTests
{
    [Theory]
    [InlineData(GenreSortBy.Name, GenreSortField.Name)]
    public void ToApplication_WhenSortByIsValid_ReturnsMappedField(
        GenreSortBy input,
        GenreSortField expected)
    {
        // Act
        var result = GenreSortMapper.ToApplication(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplication_WhenSortByIsNull_ReturnsNull()
    {
        // Act
        var result = GenreSortMapper.ToApplication(null);

        // Assert
        Assert.Null(result);
    }
}
