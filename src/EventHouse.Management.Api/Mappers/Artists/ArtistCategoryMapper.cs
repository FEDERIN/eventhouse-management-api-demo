using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class ArtistCategoryMapper
{
    public static ArtistCategoryDto ToApplicationRequired(ArtistCategory categoryContract) =>
        ApiEnumMapper<ArtistCategory, ArtistCategoryDto>.ToApplicationRequired(categoryContract);

    public static ArtistCategoryDto? ToApplicationOptional(ArtistCategory? categoryContract) =>
        ApiEnumMapper<ArtistCategory, ArtistCategoryDto>.ToApplicationOptional(categoryContract);

    public static ArtistCategory ToContract(ArtistCategoryDto category) =>
        ApiEnumMapper<ArtistCategory, ArtistCategoryDto>.ToContract(category);
}