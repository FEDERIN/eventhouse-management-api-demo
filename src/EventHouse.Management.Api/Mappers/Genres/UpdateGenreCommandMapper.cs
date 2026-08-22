using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Application.Commands.Genres.Update;

namespace EventHouse.Management.Api.Mappers.Genres;

internal static class UpdateGenreCommandMapper
{
    public static UpdateGenreCommand FromContract(Guid genreId, UpdateGenreRequest request)
        => new(genreId, request.Name);
}
