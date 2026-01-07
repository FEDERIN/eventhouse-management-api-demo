using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Delete;

internal sealed class DeleteArtistCommandHandler(IArtistRepository repository)
        : IRequestHandler<DeleteArtistCommand, DeleteResult>
{
    private readonly IArtistRepository _repository = repository;

    public async Task<DeleteResult> Handle(
        DeleteArtistCommand request,
        CancellationToken cancellationToken)
    {

        var eventEntity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (eventEntity is null)
            return DeleteResult.NotFoundResult();

        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (result is false)
            return DeleteResult.NotFoundResult();

        return DeleteResult.Ok();
    }
}
