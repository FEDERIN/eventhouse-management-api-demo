using EventHouse.Management.Application.Queries.Genres.GetById;

namespace EventHouse.Management.Api.Mappers.Genres;

internal static class GetGenreByIdQueryMapper
{
    public static GetGenreByIdQuery FromContract(Guid genreId)
        => new(genreId);
}
