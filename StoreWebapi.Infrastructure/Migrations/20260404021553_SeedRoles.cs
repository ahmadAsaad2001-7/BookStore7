using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09625b39-fde1-4b39-8983-bc7ce743d83c"), "1d76be73-238b-4e67-8197-198d7a99a56f", "VENDOR", "VENDOR" },
                    { new Guid("200bcfa4-5a90-40f0-be14-193adfac0e01"), "29f4a721-6e9e-4c9a-96d9-3b5d17f1efb9", "USER", "USER" },
                    { new Guid("ad4e1cec-240e-4fbf-82e8-018c3d0b2935"), "45d8c1ea-d70e-4ae9-8ac0-c77e09a9b420", "ADMINISTRATOR", "ADMINISTRATOR" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                column: "Version",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09625b39-fde1-4b39-8983-bc7ce743d83c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("200bcfa4-5a90-40f0-be14-193adfac0e01"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ad4e1cec-240e-4fbf-82e8-018c3d0b2935"));
        }
    }
}
