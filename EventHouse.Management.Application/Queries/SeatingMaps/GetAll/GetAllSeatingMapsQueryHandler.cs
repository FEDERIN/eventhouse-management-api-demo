using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.SeatingMaps;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

internal sealed class GetAllSeatingMapsQueryHandler(ISeatingMapRepository seatingMapRepository)
    : IRequestHandler<GetAllSeatingMapsQuery, PagedResultDto<SeatingMapDto>>
{
    public async Task<PagedResultDto<SeatingMapDto>> Handle(GetAllSeatingMapsQuery request, CancellationToken cancellationToken)
    {
        var criteria = new SeatingMapQueryCriteria
        {
            Name = request.Name,
            VenueId = request.VenueId,
            IsActive = request.IsActive,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await seatingMapRepository.GetPagedAsync(
            criteria,
            cancellationToken
        );

        return new PagedResultDto<SeatingMapDto>
        {
            Items = SeatingMapMapper.ToDto(result.Items),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
