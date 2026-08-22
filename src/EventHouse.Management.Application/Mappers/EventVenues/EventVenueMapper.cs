using EventHouse.Management.Application.Commands.EventVenues.Create;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.EventVenues;

internal static class EventVenueMapper
{
    public static EventVenue ToEntity(CreateEventVenueCommand request)
    {
        return new EventVenue(
            id: Guid.NewGuid(),
            eventId: request.EventId,
            venueId: request.VenueId,
            status: EventVenueStatusMapper.ToDomainRequired(request.Status)
        );
    }

    public static EventVenueDto ToDto(
        EventVenue entity,
        string? prefetchedEventName = null,
        string? prefetchedVenueName = null)
    {
        return new EventVenueDto
        {
            Id = entity.Id,
            EventId = entity.EventId,
            VenueId = entity.VenueId,
            EventName = prefetchedEventName ?? entity.Event?.Name,
            VenueName = prefetchedVenueName ?? entity.Venue?.Name,
            Status = EventVenueStatusMapper.ToApplicationRequired(entity.Status)
        };
    }

    public static List<EventVenueDto> ToDtoList(
        IEnumerable<EventVenue> entities,
        string? prefetchedEventName = null,
        string? prefetchedVenueName = null)
    {
        return [.. entities.Select(e => ToDto(e, prefetchedEventName, prefetchedVenueName))];
    }
}