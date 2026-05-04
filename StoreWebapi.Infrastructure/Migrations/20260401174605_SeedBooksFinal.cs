using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreWebapi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooksFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "id", "author", "description", "genres", "imageUrl", "isbn", "name", "price", "rating" ,"Version"},
                values: new object[,]
                {
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Robert C. Martin", "A Code of Conduct for Professional Programmers.", "[19,44]", "https://example.com/cleancoder.jpg", "978-0137081073", "The Clean Coder", 35.99m, 4.7999999999999998 ,1},
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "J.R.R. Tolkien", "The first volume of J.R.R. Tolkien's epic adventure.", "[23,43]", "https://example.com/lotr.jpg", "978-0547928210", "The Fellowship of the Ring", 19.99m, 4.9000000000000004 ,1}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));
        }
    }
}
