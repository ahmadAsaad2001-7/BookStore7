using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeeduserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1f5788d5-62d1-4698-b0dc-8381addac372"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("471208fa-a7f2-4d31-9ec9-30fa2f4b8631"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4f3e66f2-780c-46dc-b5a1-c628271a9bbe"));

            migrationBuilder.DropColumn(
                name: "isExpired",
                table: "Coupons");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "751a7609-a6c6-405d-8b87-12e284cbbdcf", "USER", "USER" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "f29f3849-58ff-4866-a99a-22ff67095389", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "2fd850d8-048b-4461-8647-1cc0bca5fdbb", "VENDOR", "VENDOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.AddColumn<bool>(
                name: "isExpired",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1f5788d5-62d1-4698-b0dc-8381addac372"), "cf6a8db0-ac3a-472e-ad89-3c1213dd42ea", "VENDOR", "VENDOR" },
                    { new Guid("471208fa-a7f2-4d31-9ec9-30fa2f4b8631"), "9f6dc0d8-d738-4ca2-8f78-1f8cadb313d8", "USER", "USER" },
                    { new Guid("4f3e66f2-780c-46dc-b5a1-c628271a9bbe"), "867a4a74-e262-46eb-9d25-732eff7fe3bc", "ADMINISTRATOR", "ADMINISTRATOR" }
                });
        }
    }
}
