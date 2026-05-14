using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class artistPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventVenueCalendarId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsHeadliner = table.Column<bool>(type: "INTEGER", nullable: false),
                    SetStart = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SetEnd = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPerformances", x => x.Id);
                    table.CheckConstraint("CK_ArtistPerformance_ArtistId_NotEmpty", "ArtistId <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("CK_ArtistPerformance_EventVenueCalendarId_NotEmpty", "EventVenueCalendarId <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "FK_ArtistPerformances_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistPerformances_EventVenueCalendars_EventVenueCalendarId",
                        column: x => x.EventVenueCalendarId,
                        principalTable: "EventVenueCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_ArtistPerformances_Artist_EventVenueCalendar",
                table: "ArtistPerformances",
                columns: new[] { "ArtistId", "EventVenueCalendarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ArtistPerformances_OneHeadlinerPerCalendar",
                table: "ArtistPerformances",
                column: "EventVenueCalendarId",
                unique: true,
                filter: "IsHeadliner = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistPerformances");
        }
    }
}
