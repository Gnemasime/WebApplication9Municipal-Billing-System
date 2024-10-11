using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication9Municipal_Billing_System.Migrations
{
    /// <inheritdoc />
    public partial class Due : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "waters",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "waters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "electricities",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "electricities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "waters");

            migrationBuilder.DropColumn(
                name: "status",
                table: "waters");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "electricities");

            migrationBuilder.DropColumn(
                name: "status",
                table: "electricities");
        }
    }
}
