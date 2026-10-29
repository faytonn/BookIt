using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingCodesAndTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedInAt",
                table: "ReservationSeats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedInBy",
                table: "ReservationSeats",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketCode",
                table: "ReservationSeats",
                type: "varchar(16)",
                unicode: false,
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Reservations",
                type: "varchar(9)",
                unicode: false,
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            // Existing rows got "" above; give each one a random code (same alphabet as TicketCodes)
            // before the unique indexes are created. NEWID() is evaluated per row and per character.
            migrationBuilder.Sql($"UPDATE Reservations SET Code = 'BK-' + {_randomSql(6)};");
            migrationBuilder.Sql($"UPDATE ReservationSeats SET TicketCode = {_randomSql(16)};");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeats_TicketCode",
                table: "ReservationSeats",
                column: "TicketCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Code",
                table: "Reservations",
                column: "Code",
                unique: true);
        }

        private static string _randomSql(int length)
        {
            const string alphabet = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";
            var oneChar = $"SUBSTRING('{alphabet}', ABS(CHECKSUM(NEWID())) % {alphabet.Length} + 1, 1)";
            return string.Join(" + ", Enumerable.Repeat(oneChar, length));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReservationSeats_TicketCode",
                table: "ReservationSeats");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Code",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CheckedInAt",
                table: "ReservationSeats");

            migrationBuilder.DropColumn(
                name: "CheckedInBy",
                table: "ReservationSeats");

            migrationBuilder.DropColumn(
                name: "TicketCode",
                table: "ReservationSeats");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Reservations");
        }
    }
}
