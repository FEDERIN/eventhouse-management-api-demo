using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixEventVenueMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventVenues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VenueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVenues", x => x.Id);
                    table.CheckConstraint("CK_EventVenue_EventId_NotEmpty", "EventId <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("CK_EventVenue_VenueId_NotEmpty", "VenueId <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "FK_EventVenues_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventVenues_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventVenues_VenueId",
                table: "EventVenues",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "UX_EventVenues_Event_Venue",
                table: "EventVenues",
                columns: new[] { "EventId", "VenueId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventVenues");
        }
    }
}
