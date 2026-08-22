using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

internal sealed class CreateEventVenueCalendarCommandHandler(
    IEventVenueRepository eventVenueRepository,
    IEventVenueCalendarRepository calendarEventRepository,
    ISeatingMapRepository seatingMapRepository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateEventVenueCalendarCommand, EventVenueCalendarDto>
{
    public Task<EventVenueCalendarDto> Handle(
        CreateEventVenueCalendarCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var eventVenueExists =
                    await eventVenueRepository.ExistsAsync(
                        request.EventVenueId,
                        ct);

                if (!eventVenueExists)
                    throw new NotFoundException(
                        "EventVenue",
                        request.EventVenueId);

                var startUtc = request.StartDate.UtcDateTime;
                var endUtc = request.EndDate.UtcDateTime;

                var isOccupied =
                    await calendarEventRepository.IsSlotOccupiedAsync(
                        request.EventVenueId,
                        startUtc,
                        endUtc,
                        null,
                        ct);

                if (isOccupied)
                    throw new ConflictException(
                        "CALENDAR_SLOT_OCCUPIED",
                        "Slot Occupied",
                        "The selected time slot is already occupied for this venue.");

                var seatingMapExists =
                    await seatingMapRepository.ExistsAsync(
                        request.SeatingMapId,
                        ct);

                if (!seatingMapExists)
                    throw new NotFoundException(
                        "SeatingMap",
                        request.SeatingMapId);

                var entity =
                    EventVenueCalendarMapper.ToEntity(request);

                await calendarEventRepository.AddAsync(entity, ct);

                return EventVenueCalendarMapper.ToDto(entity);
            },
            ct);
    }
}