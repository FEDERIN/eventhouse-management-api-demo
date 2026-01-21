using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
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
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (result is false)
            throw new NotFoundException("Artist", request.Id);

        return DeleteResult.Ok();
    }
}
