using EventHouse.Management.Api.Contracts.Artists;

namespace EventHouse.Management.Api.Tests.Contracts;

public static class ArtistFactory
{
    public static CreateArtistRequest CreateRequest(
        string name = "Default Artist",
        ArtistCategory category = ArtistCategory.Band) => new()
        {
            Name = name,
            Category = category
        };
}