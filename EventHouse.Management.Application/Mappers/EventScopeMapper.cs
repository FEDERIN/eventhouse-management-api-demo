
using EventHouse.Management.Application.Common.Enums;
using DomainScope = EventHouse.Management.Domain.Enums.EventScope;

namespace EventHouse.Management.Application.Mappers;

public static class EventScopeMapper
{
    public static DomainScope ToDomainRequired(EventScopeDto scope) => 
        MapToDomain(scope);

    public static DomainScope? ToDomainOptional(EventScopeDto? scope) => 
        scope is null ? null : MapToDomain(scope.Value);

    public static EventScopeDto ToApplicationRequired(DomainScope scope) => 
        MapToApplication(scope);

    private static DomainScope MapToDomain(EventScopeDto scope) => scope switch
    {
        EventScopeDto.Local => DomainScope.Local,
        EventScopeDto.National => DomainScope.National,
        EventScopeDto.International => DomainScope.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };

    private static EventScopeDto MapToApplication(DomainScope scope) => scope switch
    {
        DomainScope.Local => EventScopeDto.Local,
        DomainScope.National => EventScopeDto.National,
        DomainScope.International => EventScopeDto.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };
}
