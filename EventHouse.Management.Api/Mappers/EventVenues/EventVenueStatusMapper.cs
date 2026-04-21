using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.EventVenues;

public static class EventVenueStatusMapper
{
    public static EventVenueStatusDto ToApplicationRequired(EventVenueStatus contract) =>
    ApiEnumMapper<EventVenueStatus, EventVenueStatusDto>.ToApplicationRequired(contract);

    public static EventVenueStatusDto? ToApplicationOptional(EventVenueStatus? contract) =>
        ApiEnumMapper<EventVenueStatus, EventVenueStatusDto>.ToApplicationOptional(contract);

    public static EventVenueStatus ToContractRequired(EventVenueStatusDto dto) =>
        ApiEnumMapper<EventVenueStatus, EventVenueStatusDto>.ToContract(dto);
}
