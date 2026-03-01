using EventHouse.Management.Api.Mappers.Enums;
using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistCategory;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class ArtistCategoryMapperTests
{
    [Fact]
    public void ToApplicationOptional_WhenNull_ReturnsNull()
    {
        var result = ArtistCategoryMapper.ToApplicationOptional(null);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(Contract.Influencer, ArtistCategoryDto.Influencer)]
    [InlineData(Contract.Dancer, ArtistCategoryDto.Dancer)]
    [InlineData(Contract.Host, ArtistCategoryDto.Host)]
    [InlineData(Contract.Band, ArtistCategoryDto.Band)]
    [InlineData(Contract.Comedian, ArtistCategoryDto.Comedian)]
    [InlineData(Contract.DJ, ArtistCategoryDto.DJ)]
    [InlineData(Contract.Singer, ArtistCategoryDto.Singer)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        Contract input,
        ArtistCategoryDto expected)
    {
        Contract? nullable = input;

        var result = ArtistCategoryMapper.ToApplicationOptional(nullable);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((ArtistCategoryDto)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistCategoryMapper.ToContract(invalid));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((Contract)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArtistCategoryMapper.ToApplicationRequired(invalid));
    }
}
