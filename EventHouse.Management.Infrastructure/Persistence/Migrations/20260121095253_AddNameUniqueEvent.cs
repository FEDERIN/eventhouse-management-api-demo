using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNameUniqueEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Event_EventName_NotEmpty",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "UX_Event_Name",
                table: "Events",
                column: "Name",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Event_Name_NotEmpty",
                table: "Events",
                sql: "TRIM(Name) <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Event_Name",
                table: "Events");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Event_Name_NotEmpty",
                table: "Events");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Event_EventName_NotEmpty",
                table: "Events",
                sql: "TRIM(Name) <> ''");
        }
    }
}
