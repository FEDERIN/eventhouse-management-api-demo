using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class ArtistPerformanceSortMapper
{
    public static ArtistPerformanceSortField? ToApplication(ArtistPerformanceSortBy? sortBy) =>
    ApiEnumMapper<ArtistPerformanceSortBy, ArtistPerformanceSortField>.ToApplicationOptional(sortBy);
}
