using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Venues;

public static class VenueMapper
{
    public static Venue ToContract(VenueDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Address = dto.Address,
        City = dto.City,
        Region = dto.Region,
        CountryCode = dto.CountryCode,
        Latitude = dto.Latitude,
        Longitude = dto.Longitude,
        TimeZoneId = dto.TimeZoneId,
        Capacity = dto.Capacity,
        IsActive = dto.IsActive
    };

    public static PagedResult<Venue> ToContract(
    PagedResultDto<VenueDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
