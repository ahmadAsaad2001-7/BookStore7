using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeGenresNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "genres",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "48a4b1f5-2d59-4fd4-b29e-0f160d26d439");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ConcurrencyStamp",
                value: "0b9d3aa3-ecf0-4f60-a0a7-fc3a248b2a6d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ConcurrencyStamp",
                value: "20c0e0fd-fd06-4a10-988d-ab88b12a90b8");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                column: "genres",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                column: "genres",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "genres",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "2d5108cf-ea40-484f-81db-5fa8c0bcc729");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "ConcurrencyStamp",
                value: "f51bee74-1c4f-4535-9011-850327d1d1ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ConcurrencyStamp",
                value: "748b8b03-0a93-4e4c-8962-0ef54845da92");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                column: "genres",
                value: "[19,44]");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                column: "genres",
                value: "[23,43]");
        }
    }
}
