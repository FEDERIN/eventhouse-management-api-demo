using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTimeZoneIdInEventVenueCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "time_zone_id",
                table: "Venues",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "longitude",
                table: "Venues",
                type: "numeric(9,6)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "latitude",
                table: "Venues",
                type: "numeric(9,6)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "time_zone_id",
                table: "EventVenueCalendars",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "time_zone_id",
                table: "Venues",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<decimal>(
                name: "longitude",
                table: "Venues",
                type: "numeric(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(9,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "latitude",
                table: "Venues",
                type: "numeric(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(9,6)");

            migrationBuilder.AlterColumn<string>(
                name: "time_zone_id",
                table: "EventVenueCalendars",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
