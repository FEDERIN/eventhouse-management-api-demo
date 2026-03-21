using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Domain.Enums;


namespace EventHouse.Management.Application.Mappers.Artists;

public static class ArtistCategoryMapper
{
    public static ArtistCategory ToDomainRequired(ArtistCategoryDto category) =>
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToDomainRequired(category);

    public static ArtistCategory? ToDomainOptional(ArtistCategoryDto? category) =>
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToDomainOptional(category);

    public static ArtistCategoryDto ToApplication(ArtistCategory category) =>
        EnumMapper<ArtistCategory, ArtistCategoryDto>.ToApplicationRequired(category);
}