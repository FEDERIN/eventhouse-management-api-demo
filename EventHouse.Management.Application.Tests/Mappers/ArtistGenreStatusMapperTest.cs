using EventHouse.Management.Application.Mappers;
using DomainArtistGenreStatus = EventHouse.Management.Domain.Enums.ArtistGenreStatus;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTest
{
    [Theory]
    [InlineData(ArtistGenreStatusDto.Active, DomainArtistGenreStatus.Active)]
    [InlineData(ArtistGenreStatusDto.Inactive, DomainArtistGenreStatus.Inactive)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDto(ArtistGenreStatusDto input,DomainArtistGenreStatus expected)
    {
        var result = ArtistGenreStatusMapper.ToDomainRequired(input);
        Assert.Equal(expected, result);
    }
}
