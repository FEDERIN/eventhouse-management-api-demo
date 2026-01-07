
using EventHouse.Management.Application.Common.Enums;
using DomainCategory = EventHouse.Management.Domain.Enums.ArtistCategory;


namespace EventHouse.Management.Application.Mappers;

public static class ArtistCategoryMapper
{
    public static DomainCategory ToDomainRequired(ArtistCategory category) =>
        category switch
        {
            ArtistCategory.Singer => DomainCategory.Singer,
            ArtistCategory.Band => DomainCategory.Band,
            ArtistCategory.DJ => DomainCategory.DJ,
            ArtistCategory.Host => DomainCategory.Host,
            ArtistCategory.Comedian => DomainCategory.Comedian,
            ArtistCategory.Influencer => DomainCategory.Influencer,
            ArtistCategory.Dancer => DomainCategory.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(category),
                category,
                "Invalid ArtistCategory value."
            )
        };

    public static DomainCategory? ToDomainOptional(ArtistCategory? category) =>
        category switch
        {
            null => null,
            ArtistCategory.Singer => DomainCategory.Singer,
            ArtistCategory.Band => DomainCategory.Band,
            ArtistCategory.DJ => DomainCategory.DJ,
            ArtistCategory.Host => DomainCategory.Host,
            ArtistCategory.Comedian => DomainCategory.Comedian,
            ArtistCategory.Influencer => DomainCategory.Influencer,
            ArtistCategory.Dancer => DomainCategory.Dancer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(category),
                category,
                "Invalid ArtistCategory value."
            )
        };

    public static ArtistCategory ToApplication(DomainCategory category) =>
    category switch
    {
        DomainCategory.Singer => ArtistCategory.Singer,
        DomainCategory.Band => ArtistCategory.Band,
        DomainCategory.DJ => ArtistCategory.DJ,
        DomainCategory.Host => ArtistCategory.Host,
        DomainCategory.Comedian => ArtistCategory.Comedian,
        DomainCategory.Influencer => ArtistCategory.Influencer,
        DomainCategory.Dancer => ArtistCategory.Dancer,
        _ => throw new ArgumentOutOfRangeException(
            nameof(category),
            category,
            "Invalid ArtistCategory value."
        )
    };
}
