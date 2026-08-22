using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Delete;

public sealed record DeleteEventCommand(Guid Id) : IRequest;
