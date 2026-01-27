using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.Mappers;

public static class ArtistGenreStatusMapper
{
    public static ArtistGenreStatus ToDomainRequired(ArtistGenreStatusDto status) =>
        status switch
        {
            ArtistGenreStatusDto.Active => ArtistGenreStatus.Active,
            ArtistGenreStatusDto.Inactive => ArtistGenreStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static ArtistGenreStatus? ToDomainOptional(ArtistGenreStatusDto? status) =>
        status switch
        {
            null => null,
            ArtistGenreStatusDto.Active => ArtistGenreStatus.Active,
            ArtistGenreStatusDto.Inactive => ArtistGenreStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid ArtistGenreStatus value."
            )
        };

    public static ArtistGenreStatusDto ToApplication(ArtistGenreStatus? status) =>
    status switch
    {
        ArtistGenreStatus.Active => ArtistGenreStatusDto.Active,
        ArtistGenreStatus.Inactive => ArtistGenreStatusDto.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid ArtistGenreStatus value."
        )
    };
}
