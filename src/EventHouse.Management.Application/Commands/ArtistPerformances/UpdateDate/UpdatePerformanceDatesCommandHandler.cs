using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.UpdateDate;

internal sealed class UpdatePerformanceDatesCommandHandler(
    IEventVenueCalendarRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdatePerformanceDatesCommand>
{
    public async Task Handle(
        UpdatePerformanceDatesCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var calendar =
                    await repository.GetByIdWithPerformancesAsync(
                        request.EventVenueCalendar,
                        ct)
                    ?? throw new NotFoundException(
                        "EventVenueCalendar",
                        request.EventVenueCalendar);

                calendar.UpdatePerformance(
                    request.ArtistId,
                    request.SetStart,
                    request.SetEnd);

                await repository.UpdateAsync(
                    calendar,
                    ct);
            },
            ct);
    }
}