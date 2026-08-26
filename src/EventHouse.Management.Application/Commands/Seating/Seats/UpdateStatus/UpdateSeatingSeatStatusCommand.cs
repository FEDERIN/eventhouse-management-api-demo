using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Seats.UpdateStatus;

public sealed record UpdateSeatingSeatStatusCommand(
    Guid SeatingMapId,
    Guid SeatingSectionId,
    Guid SeatingRowId,
    Guid SeatId,
    bool IsActive) : IRequest;