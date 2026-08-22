using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.RemoveGenre;

internal sealed class RemoveArtistGenreCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<RemoveArtistGenreCommand>
{
    public async Task Handle(
        RemoveArtistGenreCommand request,
        CancellationToken ct)
    {
        var artist = await artistRepository.GetTrackedByIdAsync(request.ArtistId, ct)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        artist.RemoveGenre(request.GenreId);

        await artistRepository.UpdateAsync(artist, ct);
    }
}