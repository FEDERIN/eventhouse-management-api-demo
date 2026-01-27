using EventHouse.Management.Application.Mappers;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Application.Common.Enums;

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
}
