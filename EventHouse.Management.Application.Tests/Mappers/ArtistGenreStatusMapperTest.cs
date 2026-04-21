using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers.Artists;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTest 
    : EnumMapperTestBase<ArtistGenreStatus, ArtistGenreStatusDto>
{
    protected override ArtistGenreStatus ToDomainRequired(ArtistGenreStatusDto dto) =>
    ArtistGenreStatusMapper.ToDomainRequired(dto);

    protected override ArtistGenreStatus? ToDomainOptional(ArtistGenreStatusDto? dto) =>
        ArtistGenreStatusMapper.ToDomainOptional(dto);

    protected override ArtistGenreStatusDto ToApplicationRequired(ArtistGenreStatus domain) =>
        ArtistGenreStatusMapper.ToApplication(domain);
}
