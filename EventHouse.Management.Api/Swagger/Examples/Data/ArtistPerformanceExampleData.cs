using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Common;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class ArtistPerformanceExampleData
{
    private static readonly Guid ArtistId = ExampleConstants.ArtistId;
    private static readonly Guid ArtistPerformanceId = ExampleConstants.ArtistPerformanceId;

    internal static CreateArtistPerformanceRequest Create() => new()
    {
        ArtistId = ArtistId,
        IsHeadliner = true,
        SetStart = new DateTimeOffset(2026, 12, 6, 20, 0, 0, TimeSpan.FromHours(1)),
        SetEnd = new DateTimeOffset(2026, 12, 6, 22, 30, 0, TimeSpan.FromHours(1))
    };

    internal static UpdatePerformanceDatesRequest Update() => new()
    {
        SetStart = new DateTimeOffset(2026, 12, 6, 21, 0, 0, TimeSpan.FromHours(2)),
        SetEnd = new DateTimeOffset(2026, 12, 6, 22, 30, 0, TimeSpan.FromHours(2))
    };

    internal static ArtistPerformanceResponse Result() => new()
    {
        Id = ArtistPerformanceId,
        EventVenueCalendarId = ExampleConstants.EventVenueCalendarId,
        ArtistId = ArtistId,
        IsHeadliner = true,
        SetStart = new DateTimeOffset(2026, 12, 6, 19, 0, 0, TimeSpan.Zero),
        SetEnd = new DateTimeOffset(2026, 12, 6, 20, 30, 0, TimeSpan.Zero)
    };

    internal static GetArtistPerformancesRequest Get() => new(){
        ArtistId = ArtistId,
        IsHeadliner = true,
        Page = 1,
        PageSize = 20,
        SortBy = ArtistPerformanceSortBy.SetStart,
        SortDirection = SortDirection.Asc
    };


}
