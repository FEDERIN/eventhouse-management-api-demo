using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

internal sealed class SwapHeadlinerCommandHandler(IEventVenueCalendarRepository calendarRepository)
    : IRequestHandler<SwapHeadlinerCommand>
{
    public async Task Handle(SwapHeadlinerCommand request, CancellationToken ct = default)
    {
        var eventVenueCalendar = await calendarRepository.GetByIdWithPerformancesAsync(request.EventVenueCalendar, ct)
            ?? throw new NotFoundException(nameof(EventVenueCalendar), request.EventVenueCalendar);

        eventVenueCalendar.SwapHeadliner(request.OldArtistId, request.NewArtistId);

        await calendarRepository.SwapHeadlinerAsync(
            request.EventVenueCalendar,
            request.OldArtistId,
            request.NewArtistId,
            ct);
    }
}