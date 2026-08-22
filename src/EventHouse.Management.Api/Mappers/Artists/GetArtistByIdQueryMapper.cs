using EventHouse.Management.Application.Queries.Artists.GetById;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class GetArtistByIdQueryMapper
{
    public static GetArtistByIdQuery FromContract(Guid artistId)
        => new(artistId);
}
