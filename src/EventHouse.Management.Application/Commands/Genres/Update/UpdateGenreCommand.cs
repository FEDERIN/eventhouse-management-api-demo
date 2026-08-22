using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update;

public sealed record UpdateGenreCommand(
    Guid Id,
    string Name
    ) : IRequest;
