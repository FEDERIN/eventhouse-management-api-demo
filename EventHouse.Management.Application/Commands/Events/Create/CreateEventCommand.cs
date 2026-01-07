using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Create;

public record CreateEventCommand(
    string Name,
    string? Description,
    EventScope Scope
) : IRequest<EventDto>;
