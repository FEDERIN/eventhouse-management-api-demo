
using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistCategory;
using Dto = EventHouse.Management.Application.Common.Enums.ArtistCategory;

namespace EventHouse.Management.Api.Mappers.Enums;

public static class ArtistCategoryMapper
{
    public static Dto ToApplicationRequired(Contract categoryContract)
        => MapToApplication(categoryContract);

    public static Dto? ToApplicationOptional(Contract? categoryContract)
        => categoryContract is null ? null : MapToApplication(categoryContract.Value);

    public static Contract ToContract(Dto category)
        => category switch
        {
            Dto.Singer => Contract.Singer,
            Dto.Band => Contract.Band,
            Dto.DJ => Contract.DJ,
            Dto.Host => Contract.Host,
            Dto.Comedian => Contract.Comedian,
            Dto.Influencer => Contract.Influencer,
            Dto.Dancer => Contract.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(category),
                category,
                "Invalid ArtistCategory value."
            )
        };

    private static Dto MapToApplication(Contract categoryContract)
        => categoryContract switch
        {
            Contract.Singer => Dto.Singer,
            Contract.Band => Dto.Band,
            Contract.DJ => Dto.DJ,
            Contract.Host => Dto.Host,
            Contract.Comedian => Dto.Comedian,
            Contract.Influencer => Dto.Influencer,
            Contract.Dancer => Dto.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(categoryContract),
                categoryContract,
                "Invalid ArtistCategoryContract value."
            )
        };
}
