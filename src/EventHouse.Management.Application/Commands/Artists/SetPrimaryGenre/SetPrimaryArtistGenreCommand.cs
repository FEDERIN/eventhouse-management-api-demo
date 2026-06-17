using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

public sealed record SetPrimaryArtistGenreCommand(
    Guid ArtistId,
    Guid GenreId) : IRequest;
