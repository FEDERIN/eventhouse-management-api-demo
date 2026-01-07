using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Create;

public record CreateGenreCommand (
    string Name
) : IRequest<GenreDto>;
