using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistGenreStatus;
using EventHouse.Management.Application.Common.Enums;


namespace EventHouse.Management.Api.Mappers.Enums;


public static class ArtistGenreStatusMapper
{
    public static ArtistGenreStatusDto ToApplicationRequired(Contract statusContract) =>
        statusContract switch
        {
            Contract.Active => ArtistGenreStatusDto.Active,
            Contract.Inactive => ArtistGenreStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid ArtistGenreStatusContract value."
            )
        };

    public static ArtistGenreStatusDto? ToApplicationOptional(Contract? statusContract) =>
        statusContract switch
        {
            null => null,
            Contract.Active => ArtistGenreStatusDto.Active,
            Contract.Inactive => ArtistGenreStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid ArtistGenreStatusContract value."
            )
        };

    public static Contract ToContract(ArtistGenreStatusDto status) =>
    status switch
    {
        ArtistGenreStatusDto.Active => Contract.Active,
        ArtistGenreStatusDto.Inactive => Contract.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid ArtistGenreStatus value."
        )
    };
}
