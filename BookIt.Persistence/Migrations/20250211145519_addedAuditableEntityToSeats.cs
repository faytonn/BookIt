using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedAuditableEntityToSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Seats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Seats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 15, 55, 18, 519, DateTimeKind.Local).AddTicks(5655));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 15, 55, 18, 519, DateTimeKind.Local).AddTicks(5774));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 15, 55, 18, 519, DateTimeKind.Local).AddTicks(5779));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Seats");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 3, 31, 6, 48, DateTimeKind.Local).AddTicks(7619));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 3, 31, 6, 48, DateTimeKind.Local).AddTicks(7689));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 11, 3, 31, 6, 48, DateTimeKind.Local).AddTicks(7693));
        }
    }
}
