using Bogus;
using EventHouse.Management.Api.Contracts.Artists;

namespace EventHouse.Management.Api.Tests.Factories;

public static class ArtistFactory
{
    private static readonly Faker Faker = new();

    private static readonly Dictionary<ArtistCategory, string[]> CategoryGenres = new()
    {
        { ArtistCategory.Singer, new[] { "Pop", "Soul", "Jazz" } },
        { ArtistCategory.Band, new[] { "Rock", "Indie", "Heavy Metal" } },
        { ArtistCategory.DJ, new[] { "House", "Techno", "EDM" } },
        { ArtistCategory.Host, new[] { "Corporate", "Entertainment" } },
        { ArtistCategory.Comedian, new[] { "Stand-up", "Improvisation" } },
        { ArtistCategory.Influencer, new[] { "Lifestyle", "Gaming" } },
        { ArtistCategory.Dancer, new[] { "Salsa", "Contemporary", "Hip Hop" } }
    };

    public static CreateArtistRequest CreateRequest(
        string? name = null,
        ArtistCategory? category = null)
    {
        var selectedCategory = category ?? Faker.PickRandom<ArtistCategory>();

        return new CreateArtistRequest
        {
            Name = name ?? (selectedCategory is ArtistCategory.Singer or ArtistCategory.DJ
                ? $"{Faker.Name.FullName()} {Guid.NewGuid().ToString()[..4]}" 
                : Faker.Company.CompanyName()),
            Category = selectedCategory
        };
    }

    public static string GetRandomGenreForCategory(ArtistCategory category)
    {
        return Faker.PickRandom(CategoryGenres[category]);
    }
}