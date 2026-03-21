using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers.Events;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class EventScopeMapperTests
        : EnumMapperTestBase<EventScope, EventScopeDto>
{
    protected override EventScope ToDomainRequired(EventScopeDto dto) =>
        EventScopeMapper.ToDomainRequired(dto);

    protected override EventScope? ToDomainOptional(EventScopeDto? dto) =>
        EventScopeMapper.ToDomainOptional(dto);

    protected override EventScopeDto ToApplicationRequired(EventScope domain) =>
        EventScopeMapper.ToApplicationRequired(domain);
}
