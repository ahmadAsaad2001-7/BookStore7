using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingVendorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("18e4002a-7e85-465c-85d2-02bb6786ddff"), "39eb69e4-d1ff-4c6a-894e-6b97e8a95b09", "USER", "USER" },
                    { new Guid("3b86b32f-d510-412c-a44d-9a1d4a9ab223"), "cd208701-4bf2-4048-98e0-b055198c1585", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { new Guid("a7bf44b9-bca0-4a50-a199-a3c95205c08f"), "b029b45b-99fb-486c-911a-0d8998f3d38f", "VENDOR", "VENDOR" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                column: "UserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                column: "UserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_users_UserId",
                table: "Books",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_users_UserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("18e4002a-7e85-465c-85d2-02bb6786ddff"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3b86b32f-d510-412c-a44d-9a1d4a9ab223"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a7bf44b9-bca0-4a50-a199-a3c95205c08f"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09625b39-fde1-4b39-8983-bc7ce743d83c"), "1d76be73-238b-4e67-8197-198d7a99a56f", "VENDOR", "VENDOR" },
                    { new Guid("200bcfa4-5a90-40f0-be14-193adfac0e01"), "29f4a721-6e9e-4c9a-96d9-3b5d17f1efb9", "USER", "USER" },
                    { new Guid("ad4e1cec-240e-4fbf-82e8-018c3d0b2935"), "45d8c1ea-d70e-4ae9-8ac0-c77e09a9b420", "ADMINISTRATOR", "ADMINISTRATOR" }
                });
        }
    }
}
