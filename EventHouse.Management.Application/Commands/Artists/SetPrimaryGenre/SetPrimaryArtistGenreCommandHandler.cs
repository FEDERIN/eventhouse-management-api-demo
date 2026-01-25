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
        
        artist.SetPrimaryGenre(request.GenreId);
        
        await _artistRepository.UpdateAsync(artist, cancellationToken);
    }
}
