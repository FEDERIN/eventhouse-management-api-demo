using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

internal sealed class DeleteGenreCommandHandler(IGenreRepository repository)
       : IRequestHandler<DeleteGenreCommand, DeleteResult>
{
    private readonly IGenreRepository _repository = repository;

    public async Task<DeleteResult> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (result is false)
            throw new NotFoundException("Genre", request.Id);

        return DeleteResult.Ok();
    }
}
