using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Commands.Artists.SetGenreStatus;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class SetArtistGenreStatusCommandMapper
{
    public static SetArtistGenreStatusCommand FromContract(Guid artistId, Guid genreId, UpdateArtistGenreStatusRequest request)
        => new(artistId, genreId, ArtistGenreStatusMapper.ToApplicationRequired(request.Status));
}
