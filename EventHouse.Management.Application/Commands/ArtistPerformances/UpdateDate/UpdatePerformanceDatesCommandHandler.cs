using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.UpdateDate;

internal sealed class UpdatePerformanceDatesCommandHandler(
    IEventVenueCalendarRepository calendarRepository)
    : IRequestHandler<UpdatePerformanceDatesCommand>
{
    public async Task Handle(UpdatePerformanceDatesCommand request, CancellationToken ct)
    {
        var calendar = await calendarRepository.GetByIdWithPerformancesAsync(request.EventVenueCalendar, ct)
            ?? throw new NotFoundException("EventVenueCalendar", request.EventVenueCalendar);

        calendar.UpdatePerformance(
            request.ArtistId,
            request.SetStart,
            request.SetEnd);

        await calendarRepository.UpdateAsync(calendar, ct);
    }
}