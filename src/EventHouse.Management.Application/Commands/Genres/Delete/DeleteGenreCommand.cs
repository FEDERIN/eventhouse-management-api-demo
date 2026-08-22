using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

public sealed record DeleteGenreCommand(Guid Id) : IRequest;
