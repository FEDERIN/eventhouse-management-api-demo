using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Commands.Artists.Update;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class UpdateArtistCommandMapper
{
    public static UpdateArtistCommand FromContract(Guid artistId, UpdateArtistRequest request)
        => new(
            artistId,
            request.Name,
            ArtistCategoryMapper.ToApplicationRequired(request.Category));
}
