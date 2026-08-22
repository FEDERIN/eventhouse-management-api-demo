using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Queries.Artists.GetAll;

internal sealed class GetAllArtistsQueryHandler(IArtistRepository artistRepository) 
    : IRequestHandler<GetAllArtistsQuery, PagedResultDto<ArtistDto>>
{
    public async Task<PagedResultDto<ArtistDto>> Handle(GetAllArtistsQuery request, CancellationToken ct)
    {
        var criteria = new ArtistQueryCriteria
        {
            Name = request.Name,
            Category = ArtistCategoryMapper.ToDomainOptional(request.Category),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
        };

        var result = await artistRepository.GetPagedAsync(
            criteria,
            ct
            );

        return result.MapTo(ArtistMapper.ToDto);
    }
}