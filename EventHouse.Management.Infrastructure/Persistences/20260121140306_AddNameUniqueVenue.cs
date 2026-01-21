using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistences
{
    /// <inheritdoc />
    public partial class AddNameUniqueVenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Venues_Name",
                table: "Venues");

            migrationBuilder.CreateIndex(
                name: "UX_Venues_Name",
                table: "Venues",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Venues_Name",
                table: "Venues");

            migrationBuilder.CreateIndex(
                name: "IX_Venues_Name",
                table: "Venues",
                column: "Name");
        }
    }
}
