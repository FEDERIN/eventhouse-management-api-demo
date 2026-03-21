namespace EventHouse.Management.Api.Mappers;

internal static class ApiEnumMapper<TContract, TDto>
    where TContract : struct, Enum
    where TDto : struct, Enum
{
    public static TDto ToApplicationRequired(TContract contract)
    {
        if (!Enum.IsDefined(typeof(TContract), contract))
            throw new ArgumentOutOfRangeException(nameof(contract), contract, $"Invalid {typeof(TContract).Name} value.");

        return Enum.Parse<TDto>(contract.ToString());
    }

    public static TDto? ToApplicationOptional(TContract? contract)
    {
        if (!contract.HasValue) return null;
        return ToApplicationRequired(contract.Value);
    }

    public static TContract ToContract(TDto dto)
    {
        if (!Enum.IsDefined(typeof(TDto), dto))
            throw new ArgumentOutOfRangeException(nameof(dto), dto, $"Invalid {typeof(TDto).Name} value.");

        return Enum.Parse<TContract>(dto.ToString());
    }
}