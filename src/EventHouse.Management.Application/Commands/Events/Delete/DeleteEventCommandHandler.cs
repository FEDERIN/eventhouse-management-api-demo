using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Delete;

internal sealed class DeleteEventCommandHandler(IEventRepository repository)
            : IRequestHandler<DeleteEventCommand>
{
    public async Task Handle(
        DeleteEventCommand request,
        CancellationToken ct)
    {
        var result = await repository.DeleteAsync(request.Id, ct);
        
        if(result is false)
            throw new NotFoundException("Event", request.Id);
    }
}
