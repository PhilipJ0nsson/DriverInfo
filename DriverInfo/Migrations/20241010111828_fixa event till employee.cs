using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverInfo.Migrations
{
    /// <inheritdoc />
    public partial class fixaeventtillemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmployeeID",
                table: "Events",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Employees_EmployeeID",
                table: "Events",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Employees_EmployeeID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EmployeeID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Events");
        }
    }
}
