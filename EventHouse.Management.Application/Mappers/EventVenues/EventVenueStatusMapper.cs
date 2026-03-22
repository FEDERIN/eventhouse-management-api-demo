
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;
namespace EventHouse.Management.Application.Mappers.EventVenues;

public static class EventVenueStatusMapper
{
    public static EventVenueStatus ToDomainRequired(EventVenueStatusDto dto) =>
    EnumMapper<EventVenueStatus, EventVenueStatusDto>.ToDomainRequired(dto);

    public static EventVenueStatus? ToDomainOptional(EventVenueStatusDto? dto) =>
        EnumMapper<EventVenueStatus, EventVenueStatusDto>.ToDomainOptional(dto);

    public static EventVenueStatusDto ToApplicationRequired(EventVenueStatus domain) =>
        EnumMapper<EventVenueStatus, EventVenueStatusDto>.ToApplicationRequired(domain);
}
