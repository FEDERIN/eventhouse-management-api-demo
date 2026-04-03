using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;


namespace EventHouse.Management.Application.Mappers.Artists;

internal static class ArtistCategoryMapper
{
    public static ArtistCategory ToDomainRequired(ArtistCategoryDto category) =>
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToDomainRequired(category);

    public static ArtistCategory? ToDomainOptional(ArtistCategoryDto? category) =>
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToDomainOptional(category);

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
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToApplicationRequired(category);
}