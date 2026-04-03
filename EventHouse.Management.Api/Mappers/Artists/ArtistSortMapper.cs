using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Queries.Artists.GetAll;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class ArtistSortMapper
{
    public static ArtistSortField? ToApplication(ArtistSortBy? sortBy) =>
        ApiEnumMapper<ArtistSortBy, ArtistSortField>.ToApplicationOptional(sortBy);
}

