using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Application.Commands.Seating.Seats.Add;

namespace EventHouse.Management.Api.Mappers.Seating.Seats;

internal static class AddSeatingSeatCommandMapper
{
    public static AddSeatingSeatCommand FromContract(
        Guid seatingMapId,
        Guid seatingSectionId,
        Guid seatingRowId,
        AddSeatingSeatRequest request)
        => new(
            seatingMapId,
            seatingSectionId,
            seatingRowId,
            request.Number,
            request.Label);
}