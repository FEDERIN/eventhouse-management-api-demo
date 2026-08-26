using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Seats.Add;

public sealed record AddSeatingSeatCommand(
    Guid SeatingMapId,
    Guid SeatingSectionId,
    Guid SeatingRowId,
    int Number,
    string Label) : IRequest;