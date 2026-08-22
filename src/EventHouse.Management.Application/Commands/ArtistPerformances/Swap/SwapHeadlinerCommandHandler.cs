using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Swap;

internal sealed class SwapHeadlinerCommandHandler(
    IEventVenueCalendarRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<SwapHeadlinerCommand>
{
    public async Task Handle(
        SwapHeadlinerCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var eventVenueCalendar =
                    await repository.GetByIdWithPerformancesAsync(
                        request.EventVenueCalendar,
                        ct)
                    ?? throw new NotFoundException(
                        nameof(EventVenueCalendar),
                        request.EventVenueCalendar);

                eventVenueCalendar.ValidateHeadlinerSwap(
                    request.OldArtistId,
                    request.NewArtistId);

                await repository.SwapHeadlinerAsync(
                    request.EventVenueCalendar,
                    request.OldArtistId,
                    request.NewArtistId,
                    ct);
            },
            ct);
    }
}