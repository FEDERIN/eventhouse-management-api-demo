using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Update;

public sealed record UpdateEventCommand(
    Guid Id,
    string Name,
    string? Description,
    EventScopeDto Scope
) : IRequest;
