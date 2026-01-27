
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Mappers;

public static class EventScopeMapper
{
    public static EventScope ToDomainRequired(EventScopeDto scope) => 
        MapToDomain(scope);

    public static EventScope? ToDomainOptional(EventScopeDto? scope) => 
        scope is null ? null : MapToDomain(scope.Value);

    public static EventScopeDto ToApplicationRequired(EventScope scope) => 
        MapToApplication(scope);

    private static EventScope MapToDomain(EventScopeDto scope) => scope switch
    {
        EventScopeDto.Local => EventScope.Local,
        EventScopeDto.National => EventScope.National,
        EventScopeDto.International => EventScope.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };

    private static EventScopeDto MapToApplication(EventScope scope) => scope switch
    {
        EventScope.Local => EventScopeDto.Local,
        EventScope.National => EventScopeDto.National,
        EventScope.International => EventScopeDto.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };
}
