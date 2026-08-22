using EventHouse.Management.Application.Queries.ArtistPerformances.GetById;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class GetArtistPerformanceByIdQueryMapper
{
    public static GetArtistPerformanceByIdQuery FromContract(Guid id)
        => new(id);
}
