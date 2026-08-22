using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Delete;

internal sealed class DeleteArtistCommandHandler(IArtistRepository repository)
        : IRequestHandler<DeleteArtistCommand>
{
    public async Task Handle(
        DeleteArtistCommand request,
        CancellationToken ct)
    {
        var result = await repository.DeleteAsync(request.Id, ct);

        if (result is false)
            throw new NotFoundException("Artist", request.Id);
    }
}