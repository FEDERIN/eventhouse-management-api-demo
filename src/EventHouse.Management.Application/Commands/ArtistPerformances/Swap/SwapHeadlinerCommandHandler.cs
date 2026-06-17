using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

internal sealed class SwapHeadlinerCommandHandler(IEventVenueCalendarRepository calendarRepository)
    : IRequestHandler<SwapHeadlinerCommand>
{
    private readonly IEventVenueCalendarRepository _calendarRepository = calendarRepository;

    public async Task Handle(SwapHeadlinerCommand request, CancellationToken ct)
    {
        var eventVenueCalendar = await _calendarRepository.GetByIdWithPerformancesAsync(request.EventVenueCalendar, ct)
            ?? throw new NotFoundException(nameof(EventVenueCalendar), request.EventVenueCalendar);

        eventVenueCalendar.SwapHeadliner(request.OldArtistId, request.NewArtistId);

        await _calendarRepository.SwapHeadlinerAsync(
            request.EventVenueCalendar,
            request.OldArtistId,
            request.NewArtistId,
            ct);
    }
}