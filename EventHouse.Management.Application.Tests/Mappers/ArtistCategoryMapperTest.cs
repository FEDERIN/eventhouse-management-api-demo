using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistCategoryMapperTests
{
    [Theory]
    [InlineData(ArtistCategory.Influencer, ArtistCategoryDto.Influencer)]
    [InlineData(ArtistCategory.Dancer, ArtistCategoryDto.Dancer)]
    [InlineData(ArtistCategory.Host, ArtistCategoryDto.Host)]
    [InlineData(ArtistCategory.Band, ArtistCategoryDto.Band)]
    [InlineData(ArtistCategory.Comedian, ArtistCategoryDto.Comedian)]
    [InlineData(ArtistCategory.DJ, ArtistCategoryDto.DJ)]
    [InlineData(ArtistCategory.Singer, ArtistCategoryDto.Singer)]
    public void ToApplication_WhenValidDomainCategory_ReturnsMappedDto(
        ArtistCategory input,
        ArtistCategoryDto expected)
    {
        var result = ArtistCategoryMapper.ToApplication(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistCategoryDto.Influencer, ArtistCategory.Influencer)]
    [InlineData(ArtistCategoryDto.Dancer, ArtistCategory.Dancer)]
    [InlineData(ArtistCategoryDto.Host, ArtistCategory.Host)]
    [InlineData(ArtistCategoryDto.Band, ArtistCategory.Band)]
    [InlineData(ArtistCategoryDto.Comedian, ArtistCategory.Comedian)]
    [InlineData(ArtistCategoryDto.DJ, ArtistCategory.DJ)]
    [InlineData(ArtistCategoryDto.Singer, ArtistCategory.Singer)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDomainCategory(
        ArtistCategoryDto input,
        ArtistCategory expected)
    {
        var result = ArtistCategoryMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistCategoryDto.Influencer, ArtistCategory.Influencer)]
    [InlineData(ArtistCategoryDto.Dancer, ArtistCategory.Dancer)]
    [InlineData(ArtistCategoryDto.Host, ArtistCategory.Host)]
    [InlineData(ArtistCategoryDto.Band, ArtistCategory.Band)]
    [InlineData(ArtistCategoryDto.Comedian, ArtistCategory.Comedian)]
    [InlineData(ArtistCategoryDto.DJ, ArtistCategory.DJ)]
    [InlineData(ArtistCategoryDto.Singer, ArtistCategory.Singer)]
    public void ToDomainOptional_WhenDtoHasValue_ReturnsMappedDomainCategory(
        ArtistCategoryDto input,
        ArtistCategory expected)
    {
        var result = ArtistCategoryMapper.ToDomainOptional(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainOptional_WhenNull_ReturnsNull()
    {
        var result = ArtistCategoryMapper.ToDomainOptional(null);

        Assert.Null(result);
    }

    [Fact]
    public void ToDomainRequired_WhenInvalidValue_ShouldThrowArgumentOutOfRange()
    {
        var invalid = unchecked((ArtistCategoryDto)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistCategoryMapper.ToDomainRequired(invalid));
    }

    [Fact]
    public void ToApplication_WhenInvalidValue_ShouldThrowArgumentOutOfRange()
    {
        var invalid = unchecked((ArtistCategory)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistCategoryMapper.ToApplication(invalid));
    }
}
