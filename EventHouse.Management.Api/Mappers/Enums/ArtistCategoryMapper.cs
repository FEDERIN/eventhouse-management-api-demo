
using Contract = EventHouse.Management.Api.Contracts.Artists.ArtistCategory;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Enums;

public static class ArtistCategoryMapper
{
    public static ArtistCategoryDto ToApplicationRequired(Contract categoryContract)
        => MapToApplication(categoryContract);

    public static ArtistCategoryDto? ToApplicationOptional(Contract? categoryContract)
        => categoryContract is null ? null : MapToApplication(categoryContract.Value);

    public static Contract ToContract(ArtistCategoryDto category)
        => category switch
        {
            ArtistCategoryDto.Singer => Contract.Singer,
            ArtistCategoryDto.Band => Contract.Band,
            ArtistCategoryDto.DJ => Contract.DJ,
            ArtistCategoryDto.Host => Contract.Host,
            ArtistCategoryDto.Comedian => Contract.Comedian,
            ArtistCategoryDto.Influencer => Contract.Influencer,
            ArtistCategoryDto.Dancer => Contract.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(category),
                category,
                "Invalid ArtistCategory value."
            )
        };

    private static ArtistCategoryDto MapToApplication(Contract categoryContract)
        => categoryContract switch
        {
            Contract.Singer => ArtistCategoryDto.Singer,
            Contract.Band => ArtistCategoryDto.Band,
            Contract.DJ => ArtistCategoryDto.DJ,
            Contract.Host => ArtistCategoryDto.Host,
            Contract.Comedian => ArtistCategoryDto.Comedian,
            Contract.Influencer => ArtistCategoryDto.Influencer,
            Contract.Dancer => ArtistCategoryDto.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(categoryContract),
                categoryContract,
                "Invalid ArtistCategoryContract value."
            )
        };
}
