using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Application.Queries.Genres.GetAll;

namespace EventHouse.Management.Api.Mappers.Genres;

internal static class GenreSortMapper
{
    public static GenreSortField? ToApplication(GenreSortBy? sortBy) =>
    ApiEnumMapper<GenreSortBy, GenreSortField>.ToApplicationOptional(sortBy);
}