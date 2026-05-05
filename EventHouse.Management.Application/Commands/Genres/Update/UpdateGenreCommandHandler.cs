using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update;

internal sealed class UpdateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<UpdateGenreCommand, UpdateResult>
{
    private readonly IGenreRepository _genreRepository = genreRepository;

    public async Task<UpdateResult> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await _genreRepository.GetTrackedByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Genre", request.Id);

        entity.Update(request.Name);

        await _genreRepository.UpdateAsync(entity, cancellationToken);
        return UpdateResult.Success;
    }
}
