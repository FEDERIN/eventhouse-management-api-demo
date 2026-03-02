using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Application.Queries.Venues.GetAll;

namespace EventHouse.Management.Api.Mappers.Venues;

public static class VenueSortMapper
{
    public static VenueSortField? ToApplication(VenueSortBy? sortBy)
        => sortBy switch
        {
            VenueSortBy.Name => VenueSortField.Name,
            VenueSortBy.Address => VenueSortField.Address,
            VenueSortBy.City => VenueSortField.City,
            VenueSortBy.Region => VenueSortField.Region,
            VenueSortBy.CountryCode => VenueSortField.CountryCode,
            VenueSortBy.Capacity => VenueSortField.Capacity,
            VenueSortBy.IsActive => VenueSortField.IsActive,
            _ => null
        };
}
