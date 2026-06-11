using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

public record DeleteGenreCommand(Guid Id) : IRequest;
