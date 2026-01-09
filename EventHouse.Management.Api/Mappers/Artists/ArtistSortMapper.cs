using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Queries.Artists.GetAll;

namespace EventHouse.Management.Api.Mappers.Artists;

public static class ArtistSortMapper
{
    public static ArtistSortField? ToApplication(ArtistSortBy? sortBy)
        => sortBy switch
        {
            ArtistSortBy.Name => ArtistSortField.Name,
            ArtistSortBy.Category => ArtistSortField.Category,
            _ => null
        };
}

