using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers.Artists;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTest
{
    [Theory]
    [InlineData(ArtistGenreStatusDto.Active, ArtistGenreStatus.Active)]
    [InlineData(ArtistGenreStatusDto.Inactive, ArtistGenreStatus.Inactive)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDto(ArtistGenreStatusDto input, ArtistGenreStatus expected)
    {
        var result = ArtistGenreStatusMapper.ToDomainRequired(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistGenreStatusDto.Active, ArtistGenreStatus.Active)]
    [InlineData(ArtistGenreStatusDto.Inactive, ArtistGenreStatus.Inactive)]
    public void ToDomainOptional_WhenHasValue_ReturnsMappedDto(ArtistGenreStatusDto input, ArtistGenreStatus expected)
    {
        ArtistGenreStatusDto? nullable = input;
        var result = ArtistGenreStatusMapper.ToDomainOptional(nullable);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistGenreStatus.Active, ArtistGenreStatusDto.Active)]
    [InlineData(ArtistGenreStatus.Inactive, ArtistGenreStatusDto.Inactive)]
    public void ToApplication_WhenValidDomain_ReturnsMappedDto(ArtistGenreStatus input, ArtistGenreStatusDto expected)
    {
        var result = ArtistGenreStatusMapper.ToApplication(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainOptional_WhenNull_ReturnsNull()
    {
        var result = ArtistGenreStatusMapper.ToDomainOptional(null);
        Assert.Null(result);
    }

    [Fact]
    public void ToDomainRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatusDto)999);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToDomainRequired(invalid));
    }

    [Fact]
    public void ToDomainOptional_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatusDto)999);
        ArtistGenreStatusDto? nullable = invalid;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToDomainOptional(nullable));
    }

    [Fact]
    public void ToApplication_WhenInvalidDomain_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistGenreStatus)999);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistGenreStatusMapper.ToApplication(invalid));
    }
}
