using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Mappers.EventVenueCalendars;

internal sealed class EventVenueCalendarMapper
{
    public static EventVenueCalendarDto ToDto(EventVenueCalendar entity)
    {
        var timeZone = TZConvert.GetTimeZoneInfo(entity.TimeZoneId);
        var startLocal = TimeZoneInfo.ConvertTimeFromUtc(entity.StartDate, timeZone);
        var startOffset = new DateTimeOffset(startLocal, timeZone.GetUtcOffset(startLocal));

        DateTimeOffset? endOffset = null;
        if (entity.EndDate.HasValue)
        {
            var endLocal = TimeZoneInfo.ConvertTimeFromUtc(entity.EndDate.Value, timeZone);
            endOffset = new DateTimeOffset(endLocal, timeZone.GetUtcOffset(endLocal));
        }

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

    public static IReadOnlyList<EventVenueCalendarDto> ToDtoList(IReadOnlyList<EventVenueCalendar> entities)
    {
        return [.. entities.Select(ToDto)];
    }
}