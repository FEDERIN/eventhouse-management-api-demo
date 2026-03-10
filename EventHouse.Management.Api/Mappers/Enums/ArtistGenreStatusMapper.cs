using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Enums;

public static class ArtistGenreStatusMapper
{
    public static ArtistGenreStatusDto ToApplicationRequired(ArtistGenreStatus statusContract)
        => MapToApplication(statusContract);

    public static ArtistGenreStatusDto? ToApplicationOptional(ArtistGenreStatus? statusContract)
        => statusContract is null ? null : MapToApplication(statusContract.Value);

    public static ArtistGenreStatus ToContract(ArtistGenreStatusDto status) =>
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

    private static ArtistGenreStatusDto MapToApplication(ArtistGenreStatus statusContract) =>
        statusContract switch
        {
            ArtistGenreStatus.Active => ArtistGenreStatusDto.Active,
            ArtistGenreStatus.Inactive => ArtistGenreStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid ArtistGenreStatusContract value."
            )
        };
}
