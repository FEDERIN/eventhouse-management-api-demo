using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Add;

public sealed record AddSeatingSectionCommand(
    Guid SeatingMapId,
    string Name,
    bool IsNumbered,
    int Capacity) : IRequest;