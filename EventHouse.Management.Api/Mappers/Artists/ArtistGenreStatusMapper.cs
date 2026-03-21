using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistGenreStatus;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class ArtistGenreStatusMapper
{
    public static ArtistGenreStatusDto ToApplicationRequired(Contract statusContract) =>
        ApiEnumMapper<Contract, ArtistGenreStatusDto>.ToApplicationRequired(statusContract);

    public static ArtistGenreStatusDto? ToApplicationOptional(Contract? statusContract) =>
        ApiEnumMapper<Contract, ArtistGenreStatusDto>.ToApplicationOptional(statusContract);

    public static Contract ToContract(ArtistGenreStatusDto status) =>
        ApiEnumMapper<Contract, ArtistGenreStatusDto>.ToContract(status);
}