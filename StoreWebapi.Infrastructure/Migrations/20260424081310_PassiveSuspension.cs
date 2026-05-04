using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PassiveSuspension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SuspendedAt",
                table: "users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SuspendsUntil",
                table: "users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SuspensionReason",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuspendedAt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "SuspendsUntil",
                table: "users");

            migrationBuilder.DropColumn(
                name: "SuspensionReason",
                table: "users");
        }
    }
}
