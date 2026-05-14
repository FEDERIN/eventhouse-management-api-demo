using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

public sealed record SwapHeadlinerCommand(
    Guid EventVenueCalendar,
    Guid OldArtistId,
    Guid NewArtistId) : IRequest;