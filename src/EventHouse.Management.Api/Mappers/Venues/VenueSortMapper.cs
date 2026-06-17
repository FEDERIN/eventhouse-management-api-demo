using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Application.Queries.Venues.GetAll;

namespace EventHouse.Management.Api.Mappers.Venues;

internal static class VenueSortMapper
{
    public static VenueSortField? ToApplication(VenueSortBy? sortBy) =>
        ApiEnumMapper<VenueSortBy, VenueSortField>.ToApplicationOptional(sortBy);
}
