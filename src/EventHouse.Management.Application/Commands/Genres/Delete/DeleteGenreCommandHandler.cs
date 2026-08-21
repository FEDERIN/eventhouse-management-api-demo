using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

internal sealed class DeleteGenreCommandHandler(IGenreRepository repository)
       : IRequestHandler<DeleteGenreCommand>
{
    public async Task Handle(DeleteGenreCommand request, CancellationToken ct)
    {
        var result = await repository.DeleteAsync(request.Id, ct);

        if (result is false)
            throw new NotFoundException("Genre", request.Id);
    }
}
