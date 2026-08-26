using EventHouse.Management.Api.Contracts.Seating.Maps;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

namespace EventHouse.Management.Api.Mappers.Seating.Maps;

internal class SeatingMapSortMapper
{
    public static SeatingMapSortField? ToApplication(SeatingMapSortBy? sortBy) =>
        ApiEnumMapper<SeatingMapSortBy, SeatingMapSortField>.ToApplicationOptional(sortBy);
}
