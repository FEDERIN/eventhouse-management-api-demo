using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

internal static class SwapHeadlinerCommandMapper
{
    public static SwapHeadlinerCommand FromContract(Guid calendarId, SwapHeadlinerRequest request)
        => new(calendarId, request.OldArtistId, request.NewArtistId);
}