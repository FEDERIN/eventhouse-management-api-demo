using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_artists", x => x.id);
                    table.CheckConstraint("CK_Artist_Name_NotEmpty", "TRIM(name) <> ''");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    scope = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.CheckConstraint("CK_Event_Name_NotEmpty", "TRIM(name) <> ''");
                    table.CheckConstraint("CK_Event_Scope_Range", "Scope IN (0,1,2)");
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                    table.CheckConstraint("CK_Genre_Name_NotEmpty", "TRIM(name) <> ''");
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    country_code = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    time_zone_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venues", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ArtistGenres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    artist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_artist_genres", x => x.id);
                    table.CheckConstraint("CK_ArtistGenre_ArtistId_NotEmpty", "artist_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("CK_ArtistGenre_GenreId_NotEmpty", "genre_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_artist_genres_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "Artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_artist_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventVenues",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    venue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_venues", x => x.id);
                    table.CheckConstraint("CK_EventVenue_EventId_NotEmpty", "event_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("CK_EventVenue_VenueId_NotEmpty", "venue_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_event_venues_events_event_id",
                        column: x => x.event_id,
                        principalTable: "Events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_venues_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "Venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeatingMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    venue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seating_maps", x => x.id);
                    table.CheckConstraint("CK_SeatingMap_VenueId_NotEmpty", "venue_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_seating_maps_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "Venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventVenueCalendars",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_venue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seating_map_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    time_zone_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_venue_calendars", x => x.id);
                    table.CheckConstraint("CK_EventVenueCalendar_EndDate", "(end_date IS NULL OR end_date >= start_date)");
                    table.ForeignKey(
                        name: "fk_event_venue_calendars_event_venues_event_venue_id",
                        column: x => x.event_venue_id,
                        principalTable: "EventVenues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_venue_calendars_seating_maps_seating_map_id",
                        column: x => x.seating_map_id,
                        principalTable: "SeatingMaps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistPerformances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_venue_calendar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    artist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_headliner = table.Column<bool>(type: "boolean", nullable: false),
                    set_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    set_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_artist_performances", x => x.id);
                    table.CheckConstraint("CK_ArtistPerformance_ArtistId_NotEmpty", "artist_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("CK_ArtistPerformance_EventVenueCalendarId_NotEmpty", "event_venue_calendar_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_artist_performances_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "Artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_artist_performances_event_venue_calendars_event_venue_calend",
                        column: x => x.event_venue_calendar_id,
                        principalTable: "EventVenueCalendars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_artist_genres_genre_id",
                table: "ArtistGenres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "ux_artist_genres_artist_primary",
                table: "ArtistGenres",
                column: "artist_id",
                unique: true,
                filter: "is_primary = true");

            migrationBuilder.CreateIndex(
                name: "UX_ArtistGenres_Artist_Genre",
                table: "ArtistGenres",
                columns: new[] { "artist_id", "genre_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ArtistPerformances_Artist_EventVenueCalendar",
                table: "ArtistPerformances",
                columns: new[] { "artist_id", "event_venue_calendar_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ArtistPerformances_OneHeadlinerPerCalendar",
                table: "ArtistPerformances",
                column: "event_venue_calendar_id",
                unique: true,
                filter: "is_headliner = true");

            migrationBuilder.CreateIndex(
                name: "UX_Artists_Name",
                table: "Artists",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Event_Name",
                table: "Events",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_event_venue_calendars_event_venue_id_start_date",
                table: "EventVenueCalendars",
                columns: new[] { "event_venue_id", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ix_event_venue_calendars_seating_map_id",
                table: "EventVenueCalendars",
                column: "seating_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_EventVenueCalendar_Overlap_Search",
                table: "EventVenueCalendars",
                columns: new[] { "event_venue_id", "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_event_venues_venue_id",
                table: "EventVenues",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "UX_EventVenues_Event_Venue",
                table: "EventVenues",
                columns: new[] { "event_id", "venue_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Genres_Name",
                table: "Genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seating_maps_venue_id",
                table: "SeatingMaps",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "UX_SeatingMap_Venue_Name_Version",
                table: "SeatingMaps",
                columns: new[] { "venue_id", "name", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venues_country_code_city",
                table: "Venues",
                columns: new[] { "country_code", "city" });

            migrationBuilder.CreateIndex(
                name: "UX_Venues_Name",
                table: "Venues",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistGenres");

            migrationBuilder.DropTable(
                name: "ArtistPerformances");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "EventVenueCalendars");

            migrationBuilder.DropTable(
                name: "EventVenues");

            migrationBuilder.DropTable(
                name: "SeatingMaps");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
