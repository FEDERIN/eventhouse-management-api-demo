using DomainStatus = EventHouse.Management.Domain.Enums.ArtistGenreStatus;

using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Mappers;

public static class ArtistGenreStatusMapper
{
    public static DomainStatus ToDomainRequired(ArtistGenreStatus status) =>
        status switch
        {
            ArtistGenreStatus.Active => DomainStatus.Active,
            ArtistGenreStatus.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static DomainStatus? ToDomainOptional(ArtistGenreStatus? status) =>
        status switch
        {
            null => null,
            ArtistGenreStatus.Active => DomainStatus.Active,
            ArtistGenreStatus.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static ArtistGenreStatus ToApplication(DomainStatus? status) =>
    status switch
    {
        DomainStatus.Active => ArtistGenreStatus.Active,
        DomainStatus.Inactive => ArtistGenreStatus.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid ArtistGenreStatus value."
        )
    };
}
