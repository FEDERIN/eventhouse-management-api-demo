using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers;
using DomainCategory = EventHouse.Management.Domain.Enums.ArtistCategory;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistCategoryMapperTests
{
    [Theory]
    [InlineData(DomainCategory.Influencer, ArtistCategoryDto.Influencer)]
    [InlineData(DomainCategory.Dancer, ArtistCategoryDto.Dancer)]
    [InlineData(DomainCategory.Host, ArtistCategoryDto.Host)]
    [InlineData(DomainCategory.Band, ArtistCategoryDto.Band)]
    [InlineData(DomainCategory.Comedian, ArtistCategoryDto.Comedian)]
    [InlineData(DomainCategory.DJ, ArtistCategoryDto.DJ)]
    [InlineData(DomainCategory.Singer, ArtistCategoryDto.Singer)]
    public void ToApplication_WhenValidDomainCategory_ReturnsMappedDto(
        DomainCategory input,
        ArtistCategoryDto expected)
    {
        var result = ArtistCategoryMapper.ToApplication(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistCategoryDto.Influencer, DomainCategory.Influencer)]
    [InlineData(ArtistCategoryDto.Dancer, DomainCategory.Dancer)]
    [InlineData(ArtistCategoryDto.Host, DomainCategory.Host)]
    [InlineData(ArtistCategoryDto.Band, DomainCategory.Band)]
    [InlineData(ArtistCategoryDto.Comedian, DomainCategory.Comedian)]
    [InlineData(ArtistCategoryDto.DJ, DomainCategory.DJ)]
    [InlineData(ArtistCategoryDto.Singer, DomainCategory.Singer)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDomainCategory(
        ArtistCategoryDto input,
        DomainCategory expected)
    {
        var result = ArtistCategoryMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ArtistCategoryDto.Influencer, DomainCategory.Influencer)]
    [InlineData(ArtistCategoryDto.Dancer, DomainCategory.Dancer)]
    [InlineData(ArtistCategoryDto.Host, DomainCategory.Host)]
    [InlineData(ArtistCategoryDto.Band, DomainCategory.Band)]
    [InlineData(ArtistCategoryDto.Comedian, DomainCategory.Comedian)]
    [InlineData(ArtistCategoryDto.DJ, DomainCategory.DJ)]
    [InlineData(ArtistCategoryDto.Singer, DomainCategory.Singer)]
    public void ToDomainOptional_WhenDtoHasValue_ReturnsMappedDomainCategory(
        ArtistCategoryDto input,
        DomainCategory expected)
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
        var invalid = unchecked((DomainCategory)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistCategoryMapper.ToApplication(invalid));
    }
}
