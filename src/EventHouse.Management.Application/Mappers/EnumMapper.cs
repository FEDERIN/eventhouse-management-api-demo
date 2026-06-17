namespace EventHouse.Management.Application.Mappers;

internal static class EnumMapper<TDomain, TDto>
    where TDomain : struct, Enum
    where TDto : struct, Enum
{
    public static TDomain ToDomainRequired(TDto dto)
    {
        if (!Enum.IsDefined(typeof(TDto), dto))
            throw new ArgumentOutOfRangeException(nameof(dto), dto, $"Invalid {typeof(TDto).Name} value.");

        return Enum.Parse<TDomain>(dto.ToString());
    }

    public static TDomain? ToDomainOptional(TDto? dto)
    {
        if (!dto.HasValue) return null;
        return ToDomainRequired(dto.Value);
    }

    public static TDto ToApplicationRequired(TDomain domain)
    {
        if (!Enum.IsDefined(typeof(TDomain), domain))
            throw new ArgumentOutOfRangeException(nameof(domain), domain, $"Invalid {typeof(TDomain).Name} value.");

        return Enum.Parse<TDto>(domain.ToString());
    }
}