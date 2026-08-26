using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHouse.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatingStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "EventVenueCalendars",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SeatingSections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seating_map_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_numbered = table.Column<bool>(type: "boolean", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seating_sections", x => x.id);
                    table.CheckConstraint("CK_SeatingSection_Capacity_Positive", "capacity > 0");
                    table.CheckConstraint("CK_SeatingSection_Name_NotEmpty", "TRIM(name) <> ''");
                    table.CheckConstraint("CK_SeatingSection_SeatingMapId_NotEmpty", "seating_map_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_seating_sections_seating_maps_seating_map_id",
                        column: x => x.seating_map_id,
                        principalTable: "SeatingMaps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatingRows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seating_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seating_rows", x => x.id);
                    table.CheckConstraint("CK_SeatingRow_Label_NotEmpty", "TRIM(label) <> ''");
                    table.CheckConstraint("CK_SeatingRow_Number_Positive", "number > 0");
                    table.CheckConstraint("CK_SeatingRow_SeatingSectionId_NotEmpty", "seating_section_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_seating_rows_seating_sections_seating_section_id",
                        column: x => x.seating_section_id,
                        principalTable: "SeatingSections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seating_row_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seats", x => x.id);
                    table.CheckConstraint("CK_Seat_Label_NotEmpty", "TRIM(label) <> ''");
                    table.CheckConstraint("CK_Seat_Number_Positive", "number > 0");
                    table.CheckConstraint("CK_Seat_SeatingRowId_NotEmpty", "seating_row_id <> '00000000-0000-0000-0000-000000000000'");
                    table.ForeignKey(
                        name: "fk_seats_seating_rows_seating_row_id",
                        column: x => x.seating_row_id,
                        principalTable: "SeatingRows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_SeatingRows_SeatingSection_Number",
                table: "SeatingRows",
                columns: new[] { "seating_section_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_SeatingSections_SeatingMap_Name",
                table: "SeatingSections",
                columns: new[] { "seating_map_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Seats_SeatingRow_Number",
                table: "Seats",
                columns: new[] { "seating_row_id", "number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "SeatingRows");

            migrationBuilder.DropTable(
                name: "SeatingSections");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "EventVenueCalendars",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
