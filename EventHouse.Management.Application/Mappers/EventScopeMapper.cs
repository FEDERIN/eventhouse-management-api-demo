using AppScope = EventHouse.Management.Application.Common.Enums.EventScope;
using DomainScope = EventHouse.Management.Domain.Enums.EventScope;

namespace EventHouse.Management.Application.Mappers;

public static class EventScopeMapper
{
    // Requerido (Commands: Create / Update)
    public static DomainScope ToDomainRequired(AppScope scope) => scope switch
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

    // Opcional (Queries / Filters)
    public static DomainScope? ToDomainOptional(AppScope? scope) => scope switch
    {
        null => null,
        AppScope.Local => DomainScope.Local,
        AppScope.National => DomainScope.National,
        AppScope.International => DomainScope.International,
        _ => throw new ArgumentOutOfRangeException(
            nameof(scope),
            scope,
            "Invalid Application EventScope value."
        )
    };

    public static AppScope ToApplicationRequired(DomainScope scope) => scope switch
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
