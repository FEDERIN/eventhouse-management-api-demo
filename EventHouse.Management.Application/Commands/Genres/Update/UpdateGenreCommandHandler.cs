using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update;

internal sealed class UpdateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<UpdateGenreCommand, UpdateResult>
{
    private readonly IGenreRepository _genreRepository = genreRepository;

    public async Task<UpdateResult> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return UpdateResult.NotFound;

        entity.Update(request.Name);

        await _genreRepository.UpdateAsync(entity, cancellationToken);
        return UpdateResult.Success;
    }
}
