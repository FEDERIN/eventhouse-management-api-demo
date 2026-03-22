using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class EventScopeMapper
{
    public static EventScopeDto ToApplicationRequired(EventScope scopeContract) =>
        ApiEnumMapper<EventScope, EventScopeDto>.ToApplicationRequired(scopeContract);

    public static EventScopeDto? ToApplicationOptional(EventScope? scopeContract) =>
        ApiEnumMapper<EventScope, EventScopeDto>.ToApplicationOptional(scopeContract);

    public static EventScope ToContractRequired(EventScopeDto scope) =>
        ApiEnumMapper<EventScope, EventScopeDto>.ToContract(scope);
}