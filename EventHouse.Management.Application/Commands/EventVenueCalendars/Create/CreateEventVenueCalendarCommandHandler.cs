using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

internal sealed class CreateEventVenueCalendarCommandHandler(
    IEventVenueRepository eventVenueRepository,
    IEventVenueCalendarRepository calendarEventRepository,
    ISeatingMapRepository seatingMapRepository)
    : IRequestHandler<CreateEventVenueCalendarCommand, EventVenueCalendarDto>
{
    public async Task<EventVenueCalendarDto> Handle(
        CreateEventVenueCalendarCommand request,
        CancellationToken cancellationToken)
    {
        var eventVenueExists = await eventVenueRepository
            .ExistsAsync(request.EventVenueId, cancellationToken);
        
        if (!eventVenueExists)
            throw new NotFoundException("EventVenue", request.EventVenueId);

        var startUtc = request.StartDate.UtcDateTime;
        var endUtc = request.EndDate?.UtcDateTime
                     ?? startUtc.Date.AddDays(1).AddTicks(-1).ToUniversalTime();

        var isOccupied = await calendarEventRepository.IsSlotOccupiedAsync(
            request.EventVenueId,
            startUtc,
            endUtc,
            null,
            cancellationToken);

        if (isOccupied)
            throw new ConflictException(
                    "CALENDAR_SLOT_OCCUPIED",
                    "Slot Occupied",
                    "The selected time slot is already occupied for this venue.");


        var seatingMapExists = await seatingMapRepository
            .ExistsAsync(request.SeatingMapId, cancellationToken);

        if(!seatingMapExists)
            throw new NotFoundException("SeatingMap", request.SeatingMapId);

        var entity = new EventVenueCalendar(
            Guid.NewGuid(),
            request.EventVenueId,
            request.SeatingMapId,
            request.StartDate,
            request.EndDate,
            request.TimeZoneId,
            EventVenueCalendarStatusMapper.ToDomainRequired(request.Status)
        );

        await calendarEventRepository.AddAsync(entity, cancellationToken);

        return EventVenueCalendarMapper.ToDto(entity);
    }
}
