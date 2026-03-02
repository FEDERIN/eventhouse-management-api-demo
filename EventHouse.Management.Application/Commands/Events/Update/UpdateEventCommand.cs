using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Update;

public record UpdateEventCommand(
    Guid Id,
    string Name,
    string? Description,
    EventScopeDto Scope
) : IRequest<UpdateResult>;
