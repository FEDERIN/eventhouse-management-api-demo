using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Update;

public sealed record UpdateEventVenueCalendarCommand(
    Guid Id,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    EventVenueCalendarStatusDto Status
) : IRequest;