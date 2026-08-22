using EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class SetPrimaryArtistGenreCommandMapper
{
    public static SetPrimaryArtistGenreCommand FromContract(Guid artistId, Guid genreId)
        => new(artistId, genreId);
}
