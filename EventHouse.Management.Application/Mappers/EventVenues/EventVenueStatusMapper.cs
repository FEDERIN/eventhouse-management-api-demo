
using EventHouse.Management.Application.Common.Enums;
using DomainStatus = EventHouse.Management.Domain.Enums.EventVenueStatus;

namespace EventHouse.Management.Application.Mappers.EventVenues;

public static class EventVenueStatusMapper
{
    public static DomainStatus ToDomainRequired(EventVenueStatusDto status) =>
        status switch
        {
            EventVenueStatusDto.Active => DomainStatus.Active,
            EventVenueStatusDto.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid EventVenueStatus value."
            )
        };

    public static DomainStatus? ToDomainOptional(EventVenueStatusDto? status) =>
        status switch
        {
            null => null,
            EventVenueStatusDto.Active => DomainStatus.Active,
            EventVenueStatusDto.Inactive => DomainStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Invalid EventVenueStatus value."
            )
        };

    // Domain -> App
    public static EventVenueStatusDto ToApplicationRequired(this DomainStatus status) =>
        status switch
        {
            DomainStatus.Active => EventVenueStatusDto.Active,
            DomainStatus.Inactive => EventVenueStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid DomainStatus value.")
        };
}
