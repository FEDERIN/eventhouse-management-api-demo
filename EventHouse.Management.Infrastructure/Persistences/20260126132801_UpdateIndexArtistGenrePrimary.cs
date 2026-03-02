using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistences
{
    /// <inheritdoc />
    public partial class UpdateIndexArtistGenrePrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ArtistGenres_Artist_Primary",
                table: "ArtistGenres");

            migrationBuilder.CreateIndex(
                name: "UX_ArtistGenres_Artist_Primary",
                table: "ArtistGenres",
                column: "ArtistId",
                unique: true,
                filter: "IsPrimary = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ArtistGenres_Artist_Primary",
                table: "ArtistGenres");

            migrationBuilder.CreateIndex(
                name: "UX_ArtistGenres_Artist_Primary",
                table: "ArtistGenres",
                columns: new[] { "ArtistId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");
        }
    }
}
