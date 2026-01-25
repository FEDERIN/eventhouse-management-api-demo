using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.RemoveGenre
{
    public sealed record RemoveArtistGenreCommand(
        Guid ArtistId,
        Guid GenreId) : IRequest;
}
