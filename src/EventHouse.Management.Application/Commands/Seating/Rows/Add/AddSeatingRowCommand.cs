using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Rows.Add;

public sealed record AddSeatingRowCommand(
    Guid SeatingMapId,
    Guid SeatingSectionId,
    int Number,
    string Label) : IRequest;