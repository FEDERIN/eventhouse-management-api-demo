using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update;

internal sealed class UpdateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<UpdateGenreCommand, UpdateResult>
{
    public async Task<UpdateResult> Handle(UpdateGenreCommand request, CancellationToken ct)
    {
        var entity = await genreRepository.GetTrackedByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Genre", request.Id);

        entity.Update(request.Name);

        await genreRepository.UpdateAsync(entity, ct);
        return UpdateResult.Success;
    }
}
