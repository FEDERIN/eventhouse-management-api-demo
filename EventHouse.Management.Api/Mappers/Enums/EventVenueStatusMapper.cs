
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Enums;

public static class EventVenueStatusMapper
{
    public static EventVenueStatusDto ToApplicationRequired(EventVenueStatus statusContract) =>
        statusContract switch
        {
            EventVenueStatus.Active => EventVenueStatusDto.Active,
            EventVenueStatus.Inactive => EventVenueStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid EventVenueStatusContract value."
            )
        };

    public static EventVenueStatusDto? ToApplicationOptional(EventVenueStatus? statusContract) =>
        statusContract switch
        {
            null => null,
            EventVenueStatus.Active => EventVenueStatusDto.Active,
            EventVenueStatus.Inactive => EventVenueStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(
                nameof(statusContract),
                statusContract,
                "Invalid EventVenueStatusContract value."
            )
        };

    public static EventVenueStatus ToContractRequired(EventVenueStatusDto status) =>
    status switch
    {
        EventVenueStatusDto.Active => EventVenueStatus.Active,
        EventVenueStatusDto.Inactive => EventVenueStatus.Inactive,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Invalid EventVenueStatus value."
        )
    };
}
