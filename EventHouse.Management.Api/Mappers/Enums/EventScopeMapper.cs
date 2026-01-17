using Contract = EventHouse.Management.Api.Contracts.Events.EventScope;
using Dto = EventHouse.Management.Application.Common.Enums.EventScope;


namespace EventHouse.Management.Api.Mappers.Enums;

public static class EventScopeMapper
{
    public static Dto ToApplicationRequired(Contract scopeContract)
        => MapToApplication(scopeContract);

    public static Dto? ToApplicationOptional(Contract? scopeContract)
    => scopeContract is null ? null : MapToApplication(scopeContract.Value);

    public static Contract ToContractRequired(Dto scope) =>
    scope switch
    {
        Dto.Local => Contract.Local,
        Dto.National => Contract.National,
        Dto.International => Contract.International,
        _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, "Invalid EventScope value.")
    };

    private static Dto MapToApplication(Contract scopeContract) =>
        scopeContract switch
        {
            Contract.Local => Dto.Local,
            Contract.National => Dto.National,
            Contract.International => Dto.International,
            _ => throw new ArgumentOutOfRangeException(
                nameof(scopeContract),
                scopeContract,
                "Invalid EventScopeContract value."
            )
        };
}
