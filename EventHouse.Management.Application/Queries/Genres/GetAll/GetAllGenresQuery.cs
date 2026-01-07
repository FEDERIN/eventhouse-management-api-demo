using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetAll;
public sealed record GetAllGenresQuery
: SortablePaginationQuery<GenreSortField>, IRequest<PagedResultDto<GenreDto>>
{
    public string? Name { get; init; }
}
