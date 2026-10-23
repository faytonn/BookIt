using BookIt.Domain.Entities.Common;
using BookIt.Domain.Enums;

namespace BookIt.Domain.Entities;

/// <summary>
/// One row per (event, seat): the single source of truth for whether a seat
/// can still be sold for that event. A Seat itself belongs to a Hall and is
/// reused by every event in that hall.
/// </summary>
public class EventSeat : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int SeatId { get; set; }
    public Seat Seat { get; set; } = null!;

    public EventSeatStatus Status { get; set; } = EventSeatStatus.Available;

    /// <summary>UTC. A Held seat whose HeldUntil is in the past counts as Available.</summary>
    public DateTime? HeldUntil { get; set; }
    public string? HeldByUserId { get; set; }
    public ApplicationUser? HeldBy { get; set; }

    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    /// <summary>SQL Server rowversion; EF uses it as the optimistic concurrency token.</summary>
    public byte[] RowVersion { get; set; } = [];
}
