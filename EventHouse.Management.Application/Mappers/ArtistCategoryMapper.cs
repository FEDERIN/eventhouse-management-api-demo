
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;


namespace EventHouse.Management.Application.Mappers;

public static class ArtistCategoryMapper
{
    public static ArtistCategory ToDomainRequired(ArtistCategoryDto category)
    => MapToDomain(category);

    public static ArtistCategory? ToDomainOptional(ArtistCategoryDto? category)
    => category is null ? null : MapToDomain(category.Value);

    private static ArtistCategory MapToDomain(ArtistCategoryDto category) =>
     category switch
     {
         ArtistCategoryDto.Singer => ArtistCategory.Singer,
         ArtistCategoryDto.Band => ArtistCategory.Band,
         ArtistCategoryDto.DJ => ArtistCategory.DJ,
         ArtistCategoryDto.Host => ArtistCategory.Host,
         ArtistCategoryDto.Comedian => ArtistCategory.Comedian,
         ArtistCategoryDto.Influencer => ArtistCategory.Influencer,
         ArtistCategoryDto.Dancer => ArtistCategory.Dancer,
         _ => throw new ArgumentOutOfRangeException(
             nameof(category),
             category,
             "Invalid ArtistCategory value."
         )
     };

    public static ArtistCategoryDto ToApplication(ArtistCategory category) =>
    category switch
    {
        ArtistCategory.Singer => ArtistCategoryDto.Singer,
        ArtistCategory.Band => ArtistCategoryDto.Band,
        ArtistCategory.DJ => ArtistCategoryDto.DJ,
        ArtistCategory.Host => ArtistCategoryDto.Host,
        ArtistCategory.Comedian => ArtistCategoryDto.Comedian,
        ArtistCategory.Influencer => ArtistCategoryDto.Influencer,
        ArtistCategory.Dancer => ArtistCategoryDto.Dancer,
        _ => throw new ArgumentOutOfRangeException(
            nameof(category),
            category,
            "Invalid ArtistCategory value."
        )
    };
}