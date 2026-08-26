using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Application.Commands.Seating.Rows.Add;

namespace EventHouse.Management.Api.Mappers.Seating.Rows;

internal static class AddSeatingRowCommandMapper
{
    public static AddSeatingRowCommand FromContract(
        Guid seatingMapId,
        Guid seatingSectionId,
        AddSeatingRowRequest request)
        => new(
            seatingMapId,
            seatingSectionId,
            request.Number,
            request.Label);
}