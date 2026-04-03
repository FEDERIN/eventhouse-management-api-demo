using Contract = EventHouse.Management.Api.Contracts.Events.EventScope;
using EventHouse.Management.Application.Common.Enums;


namespace EventHouse.Management.Api.Mappers.Enums;

public static class EventScopeMapper
{
    public static EventScopeDto ToApplicationRequired(Contract scopeContract)
        => MapToApplication(scopeContract);

    public static EventScopeDto? ToApplicationOptional(Contract? scopeContract)
    => scopeContract is null ? null : MapToApplication(scopeContract.Value);

    public static Contract ToContractRequired(EventScopeDto scope) =>
    scope switch
    {
        EventScopeDto.Local => Contract.Local,
        EventScopeDto.National => Contract.National,
        EventScopeDto.International => Contract.International,
        _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, "Invalid EventScope value.")
    };

    private static EventScopeDto MapToApplication(Contract scopeContract) =>
        scopeContract switch
        {
            Contract.Local => EventScopeDto.Local,
            Contract.National => EventScopeDto.National,
            Contract.International => EventScopeDto.International,
            _ => throw new ArgumentOutOfRangeException(
                nameof(scopeContract),
                scopeContract,
                "Invalid EventScopeContract value."
            )
        };
}
