using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueSeatingMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_SeatingMap_Venue_Name",
                table: "SeatingMaps");

            migrationBuilder.CreateIndex(
                name: "UX_SeatingMap_Venue_Name_Version",
                table: "SeatingMaps",
                columns: new[] { "venue_id", "name", "version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_SeatingMap_Venue_Name_Version",
                table: "SeatingMaps");

            migrationBuilder.CreateIndex(
                name: "UX_SeatingMap_Venue_Name",
                table: "SeatingMaps",
                columns: new[] { "venue_id", "name" },
                unique: true);
        }
    }
}
