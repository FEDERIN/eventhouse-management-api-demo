using EventHouse.Management.Application.Mappers;
using DomainArtistGenreStatus = EventHouse.Management.Domain.Enums.ArtistGenreStatus;
using Dto = EventHouse.Management.Application.Common.Enums.ArtistGenreStatus;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTest
{
    [Theory]
    [InlineData(Dto.Active, DomainArtistGenreStatus.Active)]
    [InlineData(Dto.Inactive, DomainArtistGenreStatus.Inactive)]
    public void ToDomainRequired_WhenValidDto_ReturnsMappedDto(Dto input,DomainArtistGenreStatus expected)
    {
        var result = ArtistGenreStatusMapper.ToDomainRequired(input);
        Assert.Equal(expected, result);
    }
}
