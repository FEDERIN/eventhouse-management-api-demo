
using EventHouse.Management.Api.Contracts.Common;

namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

public sealed record GetArtistPerformancesRequest : SortablePaginationRequest<ArtistPerformanceSortBy>
{

    public bool? IsHeadliner { get; init; }
    public Guid? ArtistId { get; init; }
}