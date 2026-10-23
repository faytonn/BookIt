using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeterministicSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "SettingDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "Value",
                value: "bookit.aze@gmail.com");

            migrationBuilder.UpdateData(
                table: "SettingDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "Value",
                value: "bookit.aze@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 23, 4, 26, 627, DateTimeKind.Local).AddTicks(3721), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 23, 4, 26, 627, DateTimeKind.Local).AddTicks(3808), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 23, 4, 26, 627, DateTimeKind.Local).AddTicks(3811), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "SettingDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "Value",
                value: "bookit.aze@gmail.com/");

            migrationBuilder.UpdateData(
                table: "SettingDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "Value",
                value: "bookit.aze@gmail.com/");
        }
    }
}
