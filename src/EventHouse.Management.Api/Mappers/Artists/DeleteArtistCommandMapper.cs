using EventHouse.Management.Application.Commands.Artists.Delete;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class DeleteArtistCommandMapper
{
    public static DeleteArtistCommand FromContract(Guid artistId)
        => new(artistId);
}
