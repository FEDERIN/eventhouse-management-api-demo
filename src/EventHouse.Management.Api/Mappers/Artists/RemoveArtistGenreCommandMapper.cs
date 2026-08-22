using EventHouse.Management.Application.Commands.Artists.RemoveGenre;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class RemoveArtistGenreCommandMapper
{
    public static RemoveArtistGenreCommand FromContract(Guid artistId, Guid genreId)
        => new(artistId, genreId);
}
