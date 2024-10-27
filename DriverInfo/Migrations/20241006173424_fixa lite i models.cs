using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverInfo.Migrations
{
    /// <inheritdoc />
    public partial class fixaliteimodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsibleEmployee",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AmountIn",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AmountOut",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "NoteDate",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "NoteDescription",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployee",
                table: "Drivers");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountIn",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOut",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ResponsibleEmployeeID",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ResponsibleEmployeeID",
                table: "Drivers",
                column: "ResponsibleEmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Employees_ResponsibleEmployeeID",
                table: "Drivers",
                column: "ResponsibleEmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Employees_ResponsibleEmployeeID",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ResponsibleEmployeeID",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AmountIn",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AmountOut",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeID",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployee",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountIn",
                table: "Drivers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOut",
                table: "Drivers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NoteDate",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NoteDescription",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployee",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
