using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Application.Queries.Events.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class EventSortMapperTests
    : ApiEnumMapperUnidirectionalTestBase<EventSortBy, EventSortField>
{
    protected override EventSortField ToApplicationRequired(EventSortBy contract) =>
        EventSortMapper.ToApplication(contract)
        ?? throw new ArgumentNullException(nameof(contract), "Mapping failed unexpectedly.");

    protected override EventSortField? ToApplicationOptional(EventSortBy? contract) =>
        EventSortMapper.ToApplication(contract);
}