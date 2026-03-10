using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EventVenueCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UX_SeatingMap_VenueId_Name",
                table: "SeatingMaps",
                newName: "UX_SeatingMap_Venue_Name");

            migrationBuilder.CreateTable(
                name: "EventVenueCalendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventVenueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SeatingMapId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TimeZoneId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVenueCalendars", x => x.Id);
                    table.CheckConstraint("CK_EventVenueCalendar_EndDate", "(EndDate IS NULL OR EndDate >= StartDate)");
                    table.ForeignKey(
                        name: "FK_EventVenueCalendars_EventVenues_EventVenueId",
                        column: x => x.EventVenueId,
                        principalTable: "EventVenues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventVenueCalendars_SeatingMaps_SeatingMapId",
                        column: x => x.SeatingMapId,
                        principalTable: "SeatingMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventVenueCalendar_Overlap_Search",
                table: "EventVenueCalendars",
                columns: new[] { "EventVenueId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EventVenueCalendars_EventVenueId_StartDate",
                table: "EventVenueCalendars",
                columns: new[] { "EventVenueId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EventVenueCalendars_SeatingMapId",
                table: "EventVenueCalendars",
                column: "SeatingMapId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventVenueCalendars");

            migrationBuilder.RenameIndex(
                name: "UX_SeatingMap_Venue_Name",
                table: "SeatingMaps",
                newName: "UX_SeatingMap_VenueId_Name");
        }
    }
}
