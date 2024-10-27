using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverInfo.Migrations
{
    /// <inheritdoc />
    public partial class drivernullabe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "DriverID",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "DriverID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "DriverID",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "DriverID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
