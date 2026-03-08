using EventHouse.Management.Api.Contracts.Events;

namespace EventHouse.Management.Api.Tests.Factories;

internal static class EventFactory
{
    private static readonly (string Name, string Description)[] EventTemplates = new[]
    {
        ("Midnight Rock Marathon", "A 12-hour non-stop rock festival featuring local and national bands."),
        ("Electronic Sunrise", "Experience the best techno and house music under the stars."),
        ("Laughter Night Live", "A stand-up comedy special with the most hilarious comedians in the country."),
        ("Corporate Innovation Summit 2026", "Where industry leaders meet to discuss the future of technology."),
        ("Urban Dance Championship", "The biggest street dance competition with international judges."),
        ("Indie Vibes Sessions", "An intimate acoustic experience with rising indie stars."),
        ("Gaming Influencer Meetup", "Connect with your favorite streamers and pro players in person."),
        ("Salsa & Soul Festival", "A vibrant fusion of tropical rhythms and soul music."),
        ("Global DJ Masters", "The ultimate EDM experience with top-tier stage production."),
        ("Jazz in the Garden", "Relaxing afternoon featuring classical and contemporary jazz quartets.")
    };

    public static CreateEventRequest CreateRequest(string? name = null, string? description = null, EventScope? scope = EventScope.National)
    {
        var (Name, Description) = EventTemplates[new Random().Next(EventTemplates.Length)];

        return new CreateEventRequest
        {
            Name = name ?? $"{Name} {Guid.NewGuid().ToString()[..4]}" ,
            Description = description ?? Description,
            Scope = scope ?? (EventScope)new Random().Next(0, 3)
        };
    }
}