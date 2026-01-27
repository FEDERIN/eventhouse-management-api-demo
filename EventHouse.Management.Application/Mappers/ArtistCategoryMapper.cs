
using EventHouse.Management.Application.Common.Enums;
using DomainCategory = EventHouse.Management.Domain.Enums.ArtistCategory;


namespace EventHouse.Management.Application.Mappers;

public static class ArtistCategoryMapper
{
    public static DomainCategory ToDomainRequired(ArtistCategoryDto category)
    => MapToDomain(category);

    public static DomainCategory? ToDomainOptional(ArtistCategoryDto? category)
    => category is null ? null : MapToDomain(category.Value);

    private static DomainCategory MapToDomain(ArtistCategoryDto category) =>
     category switch
     {
         ArtistCategoryDto.Singer => DomainCategory.Singer,
         ArtistCategoryDto.Band => DomainCategory.Band,
         ArtistCategoryDto.DJ => DomainCategory.DJ,
         ArtistCategoryDto.Host => DomainCategory.Host,
         ArtistCategoryDto.Comedian => DomainCategory.Comedian,
         ArtistCategoryDto.Influencer => DomainCategory.Influencer,
         ArtistCategoryDto.Dancer => DomainCategory.Dancer,
         _ => throw new ArgumentOutOfRangeException(
             nameof(category),
             category,
             "Invalid ArtistCategory value."
         )
     };

    public static ArtistCategoryDto ToApplication(DomainCategory category) =>
    category switch
    {
        DomainCategory.Singer => ArtistCategoryDto.Singer,
        DomainCategory.Band => ArtistCategoryDto.Band,
        DomainCategory.DJ => ArtistCategoryDto.DJ,
        DomainCategory.Host => ArtistCategoryDto.Host,
        DomainCategory.Comedian => ArtistCategoryDto.Comedian,
        DomainCategory.Influencer => ArtistCategoryDto.Influencer,
        DomainCategory.Dancer => ArtistCategoryDto.Dancer,
        _ => throw new ArgumentOutOfRangeException(
            nameof(category),
            category,
            "Invalid ArtistCategory value."
        )
    };
}