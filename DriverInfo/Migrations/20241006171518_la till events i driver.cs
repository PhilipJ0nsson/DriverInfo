using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverInfo.Migrations
{
    /// <inheritdoc />
    public partial class latilleventsidriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Events_DriverID",
                table: "Events",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "DriverID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Drivers_DriverID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_DriverID",
                table: "Events");
        }
    }
}
