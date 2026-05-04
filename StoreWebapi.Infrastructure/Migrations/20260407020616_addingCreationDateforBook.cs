using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingCreationDateforBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3f350f88-4010-4824-a866-1c777a8889bb"), "a0f9aafe-6def-4751-8db5-d26705330951", "VENDOR", "VENDOR" },
                    { new Guid("6f73d559-c454-451a-8c76-3ec419128388"), "44b72eca-1545-4311-901e-790b0fb88347", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { new Guid("b7647204-a478-45b2-9ccf-e7939aa835be"), "e4c99a28-8ab1-402e-bcbd-129b71535ad3", "USER", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f350f88-4010-4824-a866-1c777a8889bb"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6f73d559-c454-451a-8c76-3ec419128388"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b7647204-a478-45b2-9ccf-e7939aa835be"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("18e4002a-7e85-465c-85d2-02bb6786ddff"), "39eb69e4-d1ff-4c6a-894e-6b97e8a95b09", "USER", "USER" },
                    { new Guid("3b86b32f-d510-412c-a44d-9a1d4a9ab223"), "cd208701-4bf2-4048-98e0-b055198c1585", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { new Guid("a7bf44b9-bca0-4a50-a199-a3c95205c08f"), "b029b45b-99fb-486c-911a-0d8998f3d38f", "VENDOR", "VENDOR" }
                });
        }
    }
}
