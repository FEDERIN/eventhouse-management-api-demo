using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class ExampleConstants
{
    public static readonly Guid EventId = Guid.Parse("b123c456-8d89-1e12-4f45-7a7890123457");
    public static readonly string EventName = "Summer Fest 2026";

    public static readonly Guid VenueId = Guid.Parse("c123d456-9e89-2f12-5a45-8b7890123458");
    public static readonly string VenueName = "Madison Square Garden";

    public static readonly Guid GenreId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852");
}
