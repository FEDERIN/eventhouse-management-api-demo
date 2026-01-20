using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Application.Queries.Genres.GetAll;

namespace EventHouse.Management.Api.Mappers.Genres;

public static class GenreSortMapper
{
    public static GenreSortField? ToApplication(GenreSortBy? sortBy)
        => sortBy switch
        {
            GenreSortBy.Name => GenreSortField.Name,
            _ => null
        };
}