using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Mappers.Artists;

internal static class ArtistGenreStatusMapper
{
    public static ArtistGenreStatus ToDomainRequired(ArtistGenreStatusDto status) =>
        EnumMapper<ArtistGenreStatus, ArtistGenreStatusDto>.ToDomainRequired(status);

    public static ArtistGenreStatus? ToDomainOptional(ArtistGenreStatusDto? status) =>
        EnumMapper<ArtistGenreStatus, ArtistGenreStatusDto>.ToDomainOptional(status);

    public static ArtistGenreStatusDto ToApplication(ArtistGenreStatus status) =>
        EnumMapper<ArtistGenreStatus, ArtistGenreStatusDto>.ToApplicationRequired(status);
}
