using EventHouse.Management.Application.Commands.EventVenueCalendars.Create;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Mappers.EventVenueCalendars;

internal sealed class EventVenueCalendarMapper
{
    public static EventVenueCalendar ToEntity(CreateEventVenueCalendarCommand request)
    {
        return new EventVenueCalendar(
            Guid.NewGuid(),
            request.EventVenueId,
            request.SeatingMapId,
            request.StartDate,
            request.EndDate,
            request.TimeZoneId
        );
    }

    public static EventVenueCalendarDto ToDto(EventVenueCalendar entity)
    {
        var timeZone = TZConvert.GetTimeZoneInfo(entity.TimeZoneId);
        var startLocal = TimeZoneInfo.ConvertTimeFromUtc(entity.StartDate, timeZone);
        var startOffset = new DateTimeOffset(startLocal, timeZone.GetUtcOffset(startLocal));
        var endLocal = TimeZoneInfo.ConvertTimeFromUtc(entity.EndDate, timeZone);
        var endOffset = new DateTimeOffset(endLocal, timeZone.GetUtcOffset(endLocal));
        
        return new EventVenueCalendarDto
        {
            Id = entity.Id,
            EventVenueId = entity.EventVenueId,
            SeatingMapId = entity.SeatingMapId,
            StartDate = startOffset,
            EndDate = endOffset,
            TimeZoneId = entity.TimeZoneId,
            Status = EventVenueCalendarStatusMapper.ToApplicationRequired(entity.Status)
        };
    }

    public static IEnumerable<EventVenueCalendarDto> ToDto(IEnumerable<EventVenueCalendar> entities)
        => entities.Select(ToDto);
}