using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Remove;

public sealed record RemoveArtistPerformanceCommand(
    Guid EventVenueCalendarId,
    Guid ArtistId) : IRequest;