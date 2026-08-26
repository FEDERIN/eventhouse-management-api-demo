using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Application.Commands.Seating.Maps.Update;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal static class UpdateSeatingMapCommandMapper
{
    public static UpdateSeatingMapCommand FromContract(Guid seatingMapId, UpdateSeatingMapRequest request)
        => new(
            seatingMapId,
            request.Name,
            request.Version
        );
}
