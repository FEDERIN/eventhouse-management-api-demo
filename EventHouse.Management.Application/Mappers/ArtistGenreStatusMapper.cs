using DomainStatus = EventHouse.Management.Domain.Enums.ArtistGenreStatus;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Mappers;

public static class ArtistGenreStatusMapper
{
    public static DomainStatus ToDomainRequired(ArtistGenreStatusDto status) =>
        status switch
        {
            ArtistGenreStatusDto.Active => DomainStatus.Active,
            ArtistGenreStatusDto.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static DomainStatus? ToDomainOptional(ArtistGenreStatusDto? status) =>
        status switch
        {
            null => null,
            ArtistGenreStatusDto.Active => DomainStatus.Active,
            ArtistGenreStatusDto.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static ArtistGenreStatusDto ToApplication(DomainStatus? status) =>
    status switch
    {
        DomainStatus.Active => ArtistGenreStatusDto.Active,
        DomainStatus.Inactive => ArtistGenreStatusDto.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid ArtistGenreStatus value."
        )
    };
}
