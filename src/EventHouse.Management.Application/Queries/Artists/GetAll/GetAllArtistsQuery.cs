using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Artists.GetAll;

public sealed record GetAllArtistsQuery
: SortablePaginationQuery<ArtistSortField>, IRequest<PagedResultDto<ArtistDto>>
{
    public string? Name { get; init; }
    public ArtistCategoryDto? Category { get; init; }
}
