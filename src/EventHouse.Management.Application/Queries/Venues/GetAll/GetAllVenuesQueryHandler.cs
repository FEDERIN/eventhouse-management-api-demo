using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Venues;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetAll;

internal sealed class GetAllVenuesQueryHandler(IVenueRepository venueRepository)
            : IRequestHandler<GetAllVenuesQuery, PagedResultDto<VenueDto>>
{
    public async Task<PagedResultDto<VenueDto>> Handle(GetAllVenuesQuery request, CancellationToken ct)
    {
        var criteria = new VenueQueryCriteria
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Region = request.Region,
            CountryCode = request.CountryCode,
            IsActive = request.IsActive,
            Capacity = request.Capacity,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await venueRepository.GetPagedAsync(
            criteria,
            ct
        );

        return result.MapTo(VenueMapper.ToDto);
    }
}