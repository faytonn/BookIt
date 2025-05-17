using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedEventDetailBetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventDetails_GeneralLocations_LocationId",
                table: "EventDetails");

            migrationBuilder.DropIndex(
                name: "IX_EventDetails_LocationId",
                table: "EventDetails");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "EventDetails");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 23, 54, 934, DateTimeKind.Local).AddTicks(6389));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 23, 54, 934, DateTimeKind.Local).AddTicks(6446));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 23, 54, 934, DateTimeKind.Local).AddTicks(6450));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "EventDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 21, 30, 233, DateTimeKind.Local).AddTicks(3297));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 21, 30, 233, DateTimeKind.Local).AddTicks(3356));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 15, 21, 30, 233, DateTimeKind.Local).AddTicks(3359));

            migrationBuilder.CreateIndex(
                name: "IX_EventDetails_LocationId",
                table: "EventDetails",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventDetails_GeneralLocations_LocationId",
                table: "EventDetails",
                column: "LocationId",
                principalTable: "GeneralLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
