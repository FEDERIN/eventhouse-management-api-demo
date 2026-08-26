using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Rows.UpdateStatus;

public sealed record UpdateSeatingRowStatusCommand(
    Guid SeatingMapId,
    Guid SeatingSectionId,
    Guid RowId,
    bool IsActive) : IRequest;