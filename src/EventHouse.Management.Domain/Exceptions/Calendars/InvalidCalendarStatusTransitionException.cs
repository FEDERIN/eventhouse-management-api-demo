using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Domain.Exceptions.Calendars;

public sealed class InvalidCalendarStatusTransitionException(
    EventVenueCalendarStatus currentStatus,
    EventVenueCalendarStatus requestedStatus)
        : DomainException(
        $"Cannot change calendar status from " +
            $"{currentStatus} to {requestedStatus}.")
{
}