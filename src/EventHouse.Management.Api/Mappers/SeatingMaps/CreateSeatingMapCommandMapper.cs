using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Application.Commands.Seating.Maps.Create;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal static class CreateSeatingMapCommandMapper
{
    public static CreateSeatingMapCommand FromContract(CreateSeatingMapRequest request)
        => new(
                request.VenueId,
                request.Name,
                request.Version
        );
}
