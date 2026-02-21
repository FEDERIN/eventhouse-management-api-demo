using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetAll;

internal sealed class GetAllVenuesQueryHandler(IVenueRepository venueRepository)
            : IRequestHandler<GetAllVenuesQuery, PagedResultDto<VenueDto>>
{
    private readonly IVenueRepository _venueRepository = venueRepository;

    public async Task<PagedResultDto<VenueDto>> Handle(GetAllVenuesQuery request, CancellationToken cancellationToken)
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

        var result = await _venueRepository.GetPagedAsync(
            criteria,
            cancellationToken
        );

        var dtoList = result.Items.Select(e => new VenueDto
        {
            Id = e.Id,
            Name = e.Name,
            Address = e.Address,
            City = e.City,
            Region = e.Region,
            CountryCode = e.CountryCode,
            Latitude = e.Latitude,
            Longitude = e.Longitude,
            Capacity = e.Capacity,
            TimeZoneId = e.TimeZoneId,
            IsActive = e.IsActive
        }).ToList();

        return new PagedResultDto<VenueDto>
        {
            Items = dtoList,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
