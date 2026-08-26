using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Maps.UpdateStatus;

public sealed record UpdateSeatingMapStatusCommand(
    Guid SeatingMapId,
    bool IsActive) : IRequest;