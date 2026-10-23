using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureReservationSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeat_Reservations_ReservationId",
                table: "ReservationSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeat_Seats_SeatId",
                table: "ReservationSeat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationSeat",
                table: "ReservationSeat");

            migrationBuilder.DropIndex(
                name: "IX_ReservationSeat_ReservationId",
                table: "ReservationSeat");

            migrationBuilder.RenameTable(
                name: "ReservationSeat",
                newName: "ReservationSeats");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeat_SeatId",
                table: "ReservationSeats",
                newName: "IX_ReservationSeats_SeatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationSeats",
                table: "ReservationSeats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeats_ReservationId_SeatId",
                table: "ReservationSeats",
                columns: new[] { "ReservationId", "SeatId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeats_Reservations_ReservationId",
                table: "ReservationSeats",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeats_Seats_SeatId",
                table: "ReservationSeats",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeats_Reservations_ReservationId",
                table: "ReservationSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeats_Seats_SeatId",
                table: "ReservationSeats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationSeats",
                table: "ReservationSeats");

            migrationBuilder.DropIndex(
                name: "IX_ReservationSeats_ReservationId_SeatId",
                table: "ReservationSeats");

            migrationBuilder.RenameTable(
                name: "ReservationSeats",
                newName: "ReservationSeat");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeats_SeatId",
                table: "ReservationSeat",
                newName: "IX_ReservationSeat_SeatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationSeat",
                table: "ReservationSeat",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeat_ReservationId",
                table: "ReservationSeat",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeat_Reservations_ReservationId",
                table: "ReservationSeat",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeat_Seats_SeatId",
                table: "ReservationSeat",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
