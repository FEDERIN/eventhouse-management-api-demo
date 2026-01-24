using AppScope = EventHouse.Management.Application.Common.Enums.EventScope;
using DomainScope = EventHouse.Management.Domain.Enums.EventScope;

namespace EventHouse.Management.Application.Mappers;

public static class EventScopeMapper
{
    public static DomainScope ToDomainRequired(AppScope scope) => 
        MapToDomain(scope);

    public static DomainScope? ToDomainOptional(AppScope? scope) => 
        scope is null ? null : MapToDomain(scope.Value);

    public static AppScope ToApplicationRequired(DomainScope scope) => 
        MapToApplication(scope);

    private static DomainScope MapToDomain(AppScope scope) => scope switch
    {
        AppScope.Local => DomainScope.Local,
        AppScope.National => DomainScope.National,
        AppScope.International => DomainScope.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };

    private static AppScope MapToApplication(DomainScope scope) => scope switch
    {
        DomainScope.Local => AppScope.Local,
        DomainScope.National => AppScope.National,
        DomainScope.International => AppScope.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };
}
