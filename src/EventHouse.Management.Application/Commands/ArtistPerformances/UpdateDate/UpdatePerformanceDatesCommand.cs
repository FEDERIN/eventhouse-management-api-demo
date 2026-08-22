using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.UpdateDate;

public sealed record UpdatePerformanceDatesCommand(
    Guid EventVenueCalendar, 
    Guid ArtistId,
    DateTimeOffset? SetStart,
    DateTimeOffset? SetEnd) : IRequest;