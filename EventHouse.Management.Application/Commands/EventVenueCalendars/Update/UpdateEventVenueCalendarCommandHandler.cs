using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using MediatR;


namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Update;

internal sealed class UpdateEventVenueCalendarCommandHandler(
    IEventVenueCalendarRepository repository)
    : IRequestHandler<UpdateEventVenueCalendarCommand>
{
    public async Task Handle(UpdateEventVenueCalendarCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetTrackedByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("EventVenueCalendar", request.Id);

        var startUtc = request.StartDate.UtcDateTime;
        var endUtc = request.EndDate?.UtcDateTime ?? startUtc.Date.AddDays(1).AddTicks(-1).ToUniversalTime();

        var isOccupied = await repository.IsSlotOccupiedAsync(
            entity.EventVenueId,
            startUtc,
            endUtc,
            excludeId: request.Id,
            cancellationToken);

        if (isOccupied)
            throw new ConflictException("CALENDAR_SLOT_OCCUPIED", "Slot Occupied", "...");

        entity.UpdateDates(request.StartDate, request.EndDate);

        var newStatus = EventVenueCalendarStatusMapper.ToDomainRequired(request.Status);
        entity.UpdateStatus(newStatus);

        await repository.UpdateAsync(entity, cancellationToken);
    }
}
