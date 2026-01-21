using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Delete;

internal sealed class DeleteEventCommandHandler(IEventRepository repository)
            : IRequestHandler<DeleteEventCommand, DeleteResult>
{
    private readonly IEventRepository _repository = repository;

    public async Task<DeleteResult> Handle(
        DeleteEventCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);
        
        if(result is false)
            throw new NotFoundException("Event", request.Id);

        return DeleteResult.Ok();
    }
}
