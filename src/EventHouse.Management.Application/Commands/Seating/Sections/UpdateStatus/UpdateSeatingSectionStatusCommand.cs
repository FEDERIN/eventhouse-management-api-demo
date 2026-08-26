using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.UpdateStatus;

public sealed record UpdateSeatingSectionStatusCommand(
    Guid SeatingMapId,
    Guid SectionId,
    bool IsActive) : IRequest;