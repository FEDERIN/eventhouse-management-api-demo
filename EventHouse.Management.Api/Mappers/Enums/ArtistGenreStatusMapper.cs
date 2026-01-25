using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistGenreStatus;
using Dto = EventHouse.Management.Application.Common.Enums.ArtistGenreStatus;


namespace EventHouse.Management.Api.Mappers.Enums;


public static class ArtistGenreStatusMapper
{
    public static Dto ToApplicationRequired(Contract statusContract) =>
        statusContract switch
        {
            Contract.Active => Dto.Active,
            Contract.Inactive => Dto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid ArtistGenreStatusContract value."
            )
        };

    public static Dto? ToApplicationOptional(Contract? statusContract) =>
        statusContract switch
        {
            null => null,
            Contract.Active => Dto.Active,
            Contract.Inactive => Dto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid ArtistGenreStatusContract value."
            )
        };

    public static Contract ToContract(Dto status) =>
    status switch
    {
        Dto.Active => Contract.Active,
        Dto.Inactive => Contract.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid ArtistGenreStatus value."
        )
    };
}
