using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetGenreStatus;

public sealed record SetArtistGenreStatusCommand(
    Guid ArtistId,
    Guid GenreId,
    ArtistGenreStatus Status
) : IRequest;
