using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

internal sealed class DeleteArtistCommandHandler(IGenreRepository repository)
       : IRequestHandler<DeleteGenreCommand, DeleteResult>
{
    private readonly IGenreRepository _repository = repository;

    public async Task<DeleteResult> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genreEntity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (genreEntity is null)
            return DeleteResult.NotFoundResult();

        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (result is false)
            return DeleteResult.NotFoundResult();

        return DeleteResult.Ok();
    }
}
