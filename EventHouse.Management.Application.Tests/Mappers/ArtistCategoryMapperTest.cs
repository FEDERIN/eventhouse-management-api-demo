using EventHouse.Management.Application.Mappers;
using DomainCategory = EventHouse.Management.Domain.Enums.ArtistCategory;
using Dto = EventHouse.Management.Application.Common.Enums.ArtistCategory;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistCategoryMapperTests
{
    [Theory]
    [InlineData(DomainCategory.Influencer, Dto.Influencer)]
    [InlineData(DomainCategory.Dancer, Dto.Dancer)]
    [InlineData(DomainCategory.Host, Dto.Host)]
    [InlineData(DomainCategory.Band, Dto.Band)]
    [InlineData(DomainCategory.Comedian, Dto.Comedian)]
    [InlineData(DomainCategory.DJ, Dto.DJ)]
    [InlineData(DomainCategory.Singer, Dto.Singer)]
    public void ToApplication_WhenValidDomainCategory_ReturnsMappedDto(
        DomainCategory input,
        Dto expected)
    {
        var result = ArtistCategoryMapper.ToApplication(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Dto.Influencer, DomainCategory.Influencer)]
    [InlineData(Dto.Dancer, DomainCategory.Dancer)]
    [InlineData(Dto.Host, DomainCategory.Host)]
    [InlineData(Dto.Band, DomainCategory.Band)]
    [InlineData(Dto.Comedian, DomainCategory.Comedian)]
    [InlineData(Dto.DJ, DomainCategory.DJ)]
    [InlineData(Dto.Singer, DomainCategory.Singer)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDomainCategory(
        Dto input,
        DomainCategory expected)
    {
        var result = ArtistCategoryMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Dto.Influencer, DomainCategory.Influencer)]
    [InlineData(Dto.Dancer, DomainCategory.Dancer)]
    [InlineData(Dto.Host, DomainCategory.Host)]
    [InlineData(Dto.Band, DomainCategory.Band)]
    [InlineData(Dto.Comedian, DomainCategory.Comedian)]
    [InlineData(Dto.DJ, DomainCategory.DJ)]
    [InlineData(Dto.Singer, DomainCategory.Singer)]
    public void ToDomainOptional_WhenDtoHasValue_ReturnsMappedDomainCategory(
        Dto input,
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
        var invalid = unchecked((Dto)999);

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
