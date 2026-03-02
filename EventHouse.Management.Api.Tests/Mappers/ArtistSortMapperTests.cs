using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Application.Queries.Artists.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class ArtistSortMapperTests
{
    [Theory]
    [InlineData(ArtistSortBy.Name, ArtistSortField.Name)]
    [InlineData(ArtistSortBy.Category, ArtistSortField.Category)]
    public void ToApplication_WhenSortByIsValid_ReturnsMappedField(
        ArtistSortBy input,
        ArtistSortField expected)
    {
        // Act
        var result = ArtistSortMapper.ToApplication(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplication_WhenSortByIsNull_ReturnsNull()
    {
        // Act
        var result = ArtistSortMapper.ToApplication(null);

        // Assert
        Assert.Null(result);
    }
}
