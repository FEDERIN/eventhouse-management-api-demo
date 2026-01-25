using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.RemoveGenre;

internal sealed class RemoveArtistGenreCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<RemoveArtistGenreCommand>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task Handle(
        RemoveArtistGenreCommand request,
        CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetTrackedByIdAsync(request.ArtistId, cancellationToken)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        artist.RemoveGenre(request.GenreId);

        await _artistRepository.UpdateAsync(artist, cancellationToken);
    }
}
