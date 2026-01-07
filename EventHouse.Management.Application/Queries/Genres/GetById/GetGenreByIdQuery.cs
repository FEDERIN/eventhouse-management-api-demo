using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetById
{
    public sealed record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto>;
}
