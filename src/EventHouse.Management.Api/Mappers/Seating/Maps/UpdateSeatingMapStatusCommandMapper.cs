using EventHouse.Management.Api.Contracts.Seating.Maps;
using EventHouse.Management.Application.Commands.Seating.Maps.UpdateStatus;


namespace EventHouse.Management.Api.Mappers.Seating.Maps;

internal static class UpdateSeatingMapStatusCommandMapper
{
    public static UpdateSeatingMapStatusCommand FromContract(
        Guid seatingMapId,
        UpdateSeatingMapStatusRequest request)
    {
        return new UpdateSeatingMapStatusCommand(
            seatingMapId,
            request.IsActive);
    }
}