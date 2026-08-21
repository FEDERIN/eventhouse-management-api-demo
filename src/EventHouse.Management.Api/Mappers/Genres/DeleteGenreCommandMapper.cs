using EventHouse.Management.Application.Commands.Genres.Delete;

namespace EventHouse.Management.Api.Mappers.Genres;

internal static class DeleteGenreCommandMapper
{
    public static DeleteGenreCommand FromContract(Guid genreId)
        => new(genreId);
}
