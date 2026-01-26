using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

internal sealed class SetPrimaryArtistGenreCommandHandler(IArtistRepository artistRepository) :
    IRequestHandler<SetPrimaryArtistGenreCommand>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task Handle(
        SetPrimaryArtistGenreCommand request,
        CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetTrackedByIdAsync(request.ArtistId, cancellationToken)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        var genrePrimary = artist.Genres.FirstOrDefault(a => a.IsPrimary);

        var changed = artist.SetPrimaryGenre(request.GenreId);
        
        if (changed)
        {
            var genreOldId = genrePrimary == null ? Guid.Empty : genrePrimary.GenreId;
             await _artistRepository.SetPrimaryGenreAsync(artist.Id, genreOldId, request.GenreId, cancellationToken);
        }
    }
}
