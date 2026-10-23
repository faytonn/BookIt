using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventSeatDTO;

public enum HoldOutcome
{
    Success,
    /// <summary>Seat is sold, or held by someone else, or another request won the race.</summary>
    Conflict,
    NotFound,
    /// <summary>The caller's hold no longer exists or has expired (confirm/release only).</summary>
    HoldExpired
}

public class HoldResultDTO : IDTO
{
    public HoldOutcome Outcome { get; set; }
    public int? EventSeatId { get; set; }
    public int? ReservationId { get; set; }
    public DateTime? HeldUntil { get; set; }

    public bool Succeeded => Outcome == HoldOutcome.Success;

    public static HoldResultDTO Of(HoldOutcome outcome) => new() { Outcome = outcome };
}
