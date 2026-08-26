using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Update;

public sealed record UpdateSeatingSectionCommand(
    Guid SeatingMapId,
    Guid SectionId,
    string Name,
    int Capacity) : IRequest;