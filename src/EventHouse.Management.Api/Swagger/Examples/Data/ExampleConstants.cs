using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class ExampleConstants
{
    public static readonly Guid EventId = Guid.Parse("b123c456-8d89-1e12-4f45-7a7890123457");
    public static readonly string EventName = "Summer Fest 2026";

    public static readonly Guid VenueId = Guid.Parse("c123d456-9e89-2f12-5a45-8b7890123458");
    public static readonly string VenueName = "Madison Square Garden";
    public static readonly string TimeZoneId = "America/New_York";

    public static readonly Guid GenreId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852");
    public static readonly Guid SeatingMapId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid EventVenueId = Guid.Parse("a123b456-7c89-0d12-3e45-6f7890123456");

    public static readonly Guid ArtistId = Guid.Parse("1b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed");
    public static readonly Guid EventVenueCalendarId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
    public static readonly Guid ArtistPerformanceId = Guid.Parse("7a123f64-5717-4562-b3fc-2c963f66afa6");


    public static readonly Guid SeatingSectionId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid SeatingRowId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid SeatId = Guid.Parse("55555555-5555-5555-5555-555555555555");
}
