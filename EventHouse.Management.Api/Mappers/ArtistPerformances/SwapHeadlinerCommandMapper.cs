
using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;
public static class SwapHeadlinerCommandMapper
{
    public static SwapHeadlinerCommand FromContract(Guid calendarId, SwapHeadlinerRequest request)
    {
        return new SwapHeadlinerCommand(
            calendarId,
            request.OldArtistId,
            request.NewArtistId);
    }
}