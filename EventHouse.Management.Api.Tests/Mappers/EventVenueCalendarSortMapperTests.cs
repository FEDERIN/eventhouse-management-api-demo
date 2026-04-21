using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Mappers.EventVenueCalendars;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class EventVenueCalendarSortMapperTests
    : ApiEnumMapperUnidirectionalTestBase<EventVenueCalendarSortBy, EventVenueCalendarSortField>
{
    protected override EventVenueCalendarSortField ToApplicationRequired(EventVenueCalendarSortBy contract) =>
        EventVenueCalendarSortMapper.ToApplication(contract)
        ?? throw new ArgumentNullException(nameof(contract), "Mapping failed unexpectedly.");

    protected override EventVenueCalendarSortField? ToApplicationOptional(EventVenueCalendarSortBy? contract) =>
        EventVenueCalendarSortMapper.ToApplication(contract);
}
