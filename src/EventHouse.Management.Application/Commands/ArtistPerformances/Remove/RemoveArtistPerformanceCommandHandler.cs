using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Remove;

internal sealed class RemoveArtistPerformanceCommandHandler(
    IEventVenueCalendarRepository eventVenueCalendarRepository)
    : IRequestHandler<RemoveArtistPerformanceCommand>
{
    public async Task Handle(
        RemoveArtistPerformanceCommand request,
        CancellationToken ct = default)
    {
        var eventVenueCalendar = await eventVenueCalendarRepository.GetByIdWithPerformancesAsync(
            request.EventVenueCalendarId, ct)
            ?? throw new NotFoundException(nameof(EventVenueCalendar), request.EventVenueCalendarId);

        eventVenueCalendar.RemovePerformance(request.ArtistId);

        await eventVenueCalendarRepository.UpdateAsync(eventVenueCalendar, ct);
    }
}