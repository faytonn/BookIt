using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserReservationAndWaitlistRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitlistEntries_Events_EventId1",
                table: "WaitlistEntries");

            migrationBuilder.DropIndex(
                name: "IX_WaitlistEntries_EventId1",
                table: "WaitlistEntries");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EventId1",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId1",
                table: "WaitlistEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaitlistEntries_EventId1",
                table: "WaitlistEntries",
                column: "EventId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WaitlistEntries_Events_EventId1",
                table: "WaitlistEntries",
                column: "EventId1",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
