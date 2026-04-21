using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateEventVennueCaledars1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventVenueId1",
                table: "EventVenueCalendars",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventVenueCalendars_EventVenueId1",
                table: "EventVenueCalendars",
                column: "EventVenueId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventVenueCalendars_EventVenues_EventVenueId1",
                table: "EventVenueCalendars",
                column: "EventVenueId1",
                principalTable: "EventVenues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventVenueCalendars_EventVenues_EventVenueId1",
                table: "EventVenueCalendars");

            migrationBuilder.DropIndex(
                name: "IX_EventVenueCalendars_EventVenueId1",
                table: "EventVenueCalendars");

            migrationBuilder.DropColumn(
                name: "EventVenueId1",
                table: "EventVenueCalendars");
        }
    }
}
