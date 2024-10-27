using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverInfo.Migrations
{
    /// <inheritdoc />
    public partial class lol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Drivers",
                keyColumn: "DriverID",
                keyValue: 1,
                column: "NoteDate",
                value: new DateTime(2024, 10, 6, 15, 16, 3, 229, DateTimeKind.Local).AddTicks(3053));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventID",
                keyValue: 1,
                column: "EventDate",
                value: new DateTime(2024, 10, 6, 15, 16, 3, 229, DateTimeKind.Local).AddTicks(3127));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Drivers",
                keyColumn: "DriverID",
                keyValue: 1,
                column: "NoteDate",
                value: new DateTime(2024, 10, 6, 14, 27, 37, 106, DateTimeKind.Local).AddTicks(8249));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventID",
                keyValue: 1,
                column: "EventDate",
                value: new DateTime(2024, 10, 6, 14, 27, 37, 106, DateTimeKind.Local).AddTicks(8322));
        }
    }
}
