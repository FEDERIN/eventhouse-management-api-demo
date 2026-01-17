using EventHouse.Management.Api.Mappers.Enums;
using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistCategory;
using Dto = EventHouse.Management.Application.Common.Enums.ArtistCategory;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class ArtistCategoryMapperTests
{
    [Theory]
    [InlineData(Contract.Influencer, Dto.Influencer)]
    [InlineData(Contract.Dancer, Dto.Dancer)]
    [InlineData(Contract.Host, Dto.Host)]
    [InlineData(Contract.Band, Dto.Band)]
    [InlineData(Contract.Comedian, Dto.Comedian)]
    [InlineData(Contract.DJ, Dto.DJ)]
    [InlineData(Contract.Singer, Dto.Singer)]
    public void ToApplicationRequired_WhenValidContract_ReturnsMappedDto(
        Contract input,
        Dto expected)
    {
        var result = ArtistCategoryMapper.ToApplicationRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplicationOptional_WhenNull_ReturnsNull()
    {
        var result = ArtistCategoryMapper.ToApplicationOptional(null);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(Contract.Influencer, Dto.Influencer)]
    [InlineData(Contract.Dancer, Dto.Dancer)]
    [InlineData(Contract.Host, Dto.Host)]
    [InlineData(Contract.Band, Dto.Band)]
    [InlineData(Contract.Comedian, Dto.Comedian)]
    [InlineData(Contract.DJ, Dto.DJ)]
    [InlineData(Contract.Singer, Dto.Singer)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        Contract input,
        Dto expected)
    {
        Contract? nullable = input;

        var result = ArtistCategoryMapper.ToApplicationOptional(nullable);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Dto.Influencer, Contract.Influencer)]
    [InlineData(Dto.Dancer, Contract.Dancer)]
    [InlineData(Dto.Host, Contract.Host)]
    [InlineData(Dto.Band, Contract.Band)]
    [InlineData(Dto.Comedian, Contract.Comedian)]
    [InlineData(Dto.DJ, Contract.DJ)]
    [InlineData(Dto.Singer, Contract.Singer)]

    public void ToContract_WhenValidDto_ReturnsMappedContract(
        Dto input,
        Contract expected)
    {
        var result = ArtistCategoryMapper.ToContract(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((Dto)999);

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
