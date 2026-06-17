using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Add;

public sealed record AddArtistPerformanceCommand(
    Guid EventVenueCalendarId,
    Guid ArtistId,
    bool IsHeadliner,
    DateTimeOffset? SetStart,
    DateTimeOffset? SetEnd
) : IRequest<ArtistPerformanceDto>;
