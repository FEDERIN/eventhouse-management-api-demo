using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Delete;

public record DeleteEventCommand(Guid Id) : IRequest;
