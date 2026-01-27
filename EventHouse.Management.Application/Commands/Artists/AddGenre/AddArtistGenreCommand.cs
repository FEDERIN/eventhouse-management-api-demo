using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.AddGenre
{
    public sealed record AddArtistGenreCommand(
        Guid ArtistId,
        Guid GenreId,
        ArtistGenreStatusDto Status,
        bool IsPrimary) : IRequest;
}
