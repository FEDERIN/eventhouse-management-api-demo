using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTests
{
    [Theory]
    [InlineData(ArtistGenreStatus.Active, ArtistGenreStatusDto.Active)]
    [InlineData(ArtistGenreStatus.Inactive, ArtistGenreStatusDto.Inactive)]
    public void ToApplicationRequired_WhenValidContract_ReturnsMappedDto(
        ArtistGenreStatus input,
        ArtistGenreStatusDto expected)
    {
        var result = ArtistGenreStatusMapper.ToApplicationRequired(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistGenreStatus.Active, ArtistGenreStatusDto.Active)]
    [InlineData(ArtistGenreStatus.Inactive, ArtistGenreStatusDto.Inactive)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        ArtistGenreStatus input,
        ArtistGenreStatusDto expected)
    {
        ArtistGenreStatus? nullable = input;
        var result = ArtistGenreStatusMapper.ToApplicationOptional(nullable);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistGenreStatusDto.Active, ArtistGenreStatus.Active)]
    [InlineData(ArtistGenreStatusDto.Inactive, ArtistGenreStatus.Inactive)]
    public void ToContract_WhenValidDto_ReturnsMappedContract(
        ArtistGenreStatusDto input,
        ArtistGenreStatus expected)
    {
        var result = ArtistGenreStatusMapper.ToContract(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplicationOptional_WhenNull_ReturnsNull()
    {
        var result = ArtistGenreStatusMapper.ToApplicationOptional(null);
        Assert.Null(result);
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatusDto)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToContract(invalid));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatus)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToApplicationRequired(invalid));
    }

    [Fact]
    public void ToApplicationOptional_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatus)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToApplicationOptional(invalid));
    }

}
