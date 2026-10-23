using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BackfillEventSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // One EventSeat per (live event, live seat in the event's hall).
            // Seats already taken by a non-cancelled reservation start as Sold (confirmed)
            // or Held for 10 minutes (pending), so existing bookings survive the switch.
            migrationBuilder.Sql(@"
INSERT INTO [EventSeats] ([EventId], [SeatId], [Status], [HeldUntil], [HeldByUserId], [ReservationId])
SELECT e.[Id], s.[Id], 0, NULL, NULL, NULL
FROM [Events] e
JOIN [Seats] s ON s.[HallId] = e.[HallId]
WHERE e.[IsDeleted] = 0 AND s.[IsDeleted] = 0
  AND NOT EXISTS (SELECT 1 FROM [EventSeats] x WHERE x.[EventId] = e.[Id] AND x.[SeatId] = s.[Id]);

UPDATE es
SET es.[Status] = CASE WHEN r.[Status] = 0 THEN 2 ELSE 1 END,
    es.[HeldUntil] = CASE WHEN r.[Status] = 0 THEN NULL ELSE DATEADD(minute, 10, SYSUTCDATETIME()) END,
    es.[HeldByUserId] = CASE WHEN r.[Status] = 0 THEN NULL ELSE r.[UserId] END,
    es.[ReservationId] = r.[Id]
FROM [EventSeats] es
JOIN [ReservationSeats] rs ON rs.[SeatId] = es.[SeatId] AND rs.[IsDeleted] = 0
JOIN [Reservations] r ON r.[Id] = rs.[ReservationId] AND r.[EventId] = es.[EventId]
WHERE r.[IsDeleted] = 0 AND r.[Status] IN (0, 2);
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [EventSeats];");

        }
    }
}
