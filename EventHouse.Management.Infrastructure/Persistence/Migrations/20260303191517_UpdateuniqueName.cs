using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateuniqueName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Artists_Name",
                table: "Artists",
                newName: "UX_Artists_Name");

            migrationBuilder.CreateIndex(
                name: "UX_SeatingMap_VenueId_Name",
                table: "SeatingMaps",
                columns: new[] { "VenueId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_SeatingMap_VenueId_Name",
                table: "SeatingMaps");

            migrationBuilder.RenameIndex(
                name: "UX_Artists_Name",
                table: "Artists",
                newName: "IX_Artists_Name");
        }
    }
}
