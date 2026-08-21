using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class GetAllArtistPerformancesQueryMapper
{
    public static GetAllArtistPerformancesQuery FromContract(Guid eventVenueCalendarId,
    GetArtistPerformancesRequest request)
        => new (eventVenueCalendarId)
        {
            ArtistId = request.ArtistId,
            IsHeadliner = request.IsHeadliner,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = ArtistPerformanceSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}
