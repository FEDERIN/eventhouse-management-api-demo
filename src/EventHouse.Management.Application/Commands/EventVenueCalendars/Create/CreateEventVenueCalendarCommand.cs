using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

public sealed record CreateEventVenueCalendarCommand(
    Guid EventVenueId,
    Guid SeatingMapId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string TimeZoneId
) : IRequest<EventVenueCalendarDto>;
