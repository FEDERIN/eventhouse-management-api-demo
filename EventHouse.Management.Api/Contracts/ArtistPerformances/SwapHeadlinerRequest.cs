namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

public sealed record SwapHeadlinerRequest(
    Guid OldArtistId,
    Guid NewArtistId);