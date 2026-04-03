using EventHouse.Management.Api.Mappers.Enums;
using Contract = EventHouse.Management.Api.Contracts.Events.EventScope;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Api.Mappers.Events;
using EventHouse.Management.Api.Contracts.Events;
using EventScopeMapper = EventHouse.Management.Api.Mappers.Enums.EventScopeMapper;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class EventScopeMapperTests
    : ApiEnumMapperTestBase<EventScope, EventScopeDto>
{
    protected override EventScopeDto ToApplicationRequired(EventScope contract) =>
        EventScopeMapper.ToApplicationRequired(contract);

    protected override EventScopeDto? ToApplicationOptional(EventScope? contract) =>
        EventScopeMapper.ToApplicationOptional(contract);

    protected override EventScope ToContractRequired(EventScopeDto dto) =>
        EventScopeMapper.ToContractRequired(dto);
}
