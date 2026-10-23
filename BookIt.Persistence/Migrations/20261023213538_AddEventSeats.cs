using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEventSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Seats");

            migrationBuilder.CreateTable(
                name: "EventSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HeldUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReservationId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSeats_AspNetUsers_HeldByUserId",
                        column: x => x.HeldByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventSeats_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSeats_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_EventId_SeatId",
                table: "EventSeats",
                columns: new[] { "EventId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_EventId_Status",
                table: "EventSeats",
                columns: new[] { "EventId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_HeldByUserId",
                table: "EventSeats",
                column: "HeldByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_HeldUntil",
                table: "EventSeats",
                column: "HeldUntil",
                filter: "[Status] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_ReservationId",
                table: "EventSeats",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeats_SeatId",
                table: "EventSeats",
                column: "SeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSeats");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
