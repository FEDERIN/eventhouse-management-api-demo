using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Application.Commands.Seating.Seats.UpdateStatus;

namespace EventHouse.Management.Api.Mappers.Seating.Seats;

internal static class UpdateSeatingSeatStatusCommandMapper
{
    public static UpdateSeatingSeatStatusCommand FromContract(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId,
        Guid seatId,
        UpdateSeatingSeatStatusRequest request)
    {
        return new UpdateSeatingSeatStatusCommand(
            seatingMapId,
            sectionId,
            rowId,
            seatId,
            request.IsActive);
    }
}