using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Mappers.Events;

public static class EventScopeMapper
{
    public static EventScope ToDomainRequired(EventScopeDto scope) =>
        EnumMapper<EventScope, EventScopeDto>.ToDomainRequired(scope);

    public static EventScope? ToDomainOptional(EventScopeDto? scope) =>
        EnumMapper<EventScope, EventScopeDto>.ToDomainOptional(scope);

    public static EventScopeDto ToApplicationRequired(EventScope scope) =>
        EnumMapper<EventScope, EventScopeDto>.ToApplicationRequired(scope);
}