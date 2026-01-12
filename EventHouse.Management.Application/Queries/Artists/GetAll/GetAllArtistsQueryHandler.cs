using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;
using EventHouse.Management.Application.Mappers;

namespace EventHouse.Management.Application.Queries.Artists.GetAll;

internal sealed class GetAllArtistsQueryHandler(IArtistRepository artistRepository) 
    : IRequestHandler<GetAllArtistsQuery, PagedResultDto<ArtistDto>>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task<PagedResultDto<ArtistDto>> Handle(GetAllArtistsQuery request, CancellationToken cancellationToken)
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

        var result = await _artistRepository.GetPagedAsync(
            criteria,
            cancellationToken
            );

        var dtoList = result.Items.Select(e => new ArtistDto
        {
            Id = e.Id,
            Name = e.Name,
            Category = ArtistCategoryMapper.ToApplication(e.Category)
        }).ToList();

        return new PagedResultDto<ArtistDto>
        {
            Items = dtoList,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
