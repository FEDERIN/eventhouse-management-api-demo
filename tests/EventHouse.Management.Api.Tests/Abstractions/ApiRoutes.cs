
namespace EventHouse.Management.Api.Tests.Abstractions;

internal static class ApiRoutes
{
    private const string Base = "/api/v1";
    public const string Genres = $"{Base}/genres";
    public const string Artists = $"{Base}/artists";
    public const string Venues = $"{Base}/venues";
    public const string Events = $"{Base}/events";
    public const string SeatingMaps = $"{Base}/seatingmaps";
    public const string EventVenues = $"{Base}/event-venues";
    public const string EventVenueCalendars = $"{Base}/event-venue-calendars";
    public const string ArtistPerformances = $"{Base}/artist-performances";

    public static string SeatingSections(Guid seatingMapId) =>
        $"{SeatingMaps}/{seatingMapId}/sections";

    public static string SeatingRows(Guid seatingMapId, Guid sectionId) =>
        $"{SeatingSections(seatingMapId)}/{sectionId}/rows";

    public static string SeatingSeats(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId) =>
        $"{SeatingRows(seatingMapId, sectionId)}/{rowId}/seats";
}