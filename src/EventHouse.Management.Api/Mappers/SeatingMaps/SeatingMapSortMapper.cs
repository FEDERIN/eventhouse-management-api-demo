using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal class SeatingMapSortMapper
{
    public static SeatingMapSortField? ToApplication(SeatingMapSortBy? sortBy) =>
        ApiEnumMapper<SeatingMapSortBy, SeatingMapSortField>.ToApplicationOptional(sortBy);
}
