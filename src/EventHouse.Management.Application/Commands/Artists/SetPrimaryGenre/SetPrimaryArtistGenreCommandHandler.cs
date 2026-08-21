using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

internal sealed class SetPrimaryArtistGenreCommandHandler(IArtistRepository artistRepository) :
    IRequestHandler<SetPrimaryArtistGenreCommand>
{
    public async Task Handle(
        SetPrimaryArtistGenreCommand request,
        CancellationToken ct)
    {
        var artist = await artistRepository.GetByIdAsync(request.ArtistId, ct)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        var genrePrimary = artist.Genres.FirstOrDefault(a => a.IsPrimary);

        var changed = artist.SetPrimaryGenre(request.GenreId);
        
        if (changed)
        {
            var genreOldId = genrePrimary == null ? Guid.Empty : genrePrimary.GenreId;
             await artistRepository.SetPrimaryGenreAsync(artist.Id, genreOldId, request.GenreId, ct);
        }
    }
}