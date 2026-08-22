using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Commands.Artists.AddGenre;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class AddArtistGenreCommandMapper
{
    public static AddArtistGenreCommand FromContract(Guid artistId, AddArtistGenreRequest request)
        => new(artistId, request.GenreId, ArtistGenreStatusMapper.ToApplicationRequired(request.Status), request.IsPrimary);
}
