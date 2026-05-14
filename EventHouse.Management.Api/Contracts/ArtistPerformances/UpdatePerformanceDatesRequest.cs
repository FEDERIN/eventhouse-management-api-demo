namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

public sealed record UpdatePerformanceDatesRequest
{
    // <summary>
    // Gets or initilizes the start time of the artist's set.
    // </summary>
    public DateTimeOffset? SetStart { get; init; }
    // <summary>
    // Gets or initilizes the end time of the artist's set.
    // </summary>
    public DateTimeOffset? SetEnd { get; init; }
}
