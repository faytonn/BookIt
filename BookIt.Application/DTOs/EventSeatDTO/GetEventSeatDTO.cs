using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.EventSeatDTO;

public class GetEventSeatDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int SeatId { get; set; }
    public string SeatName { get; set; } = null!;
    public int Row { get; set; }
    public int Column { get; set; }
    public string SeatTypeName { get; set; } = null!;
    public decimal Price { get; set; }

    /// <summary>Effective status: an expired hold is reported as Available.</summary>
    public EventSeatStatus Status { get; set; }
    public DateTime? HeldUntil { get; set; }

    /// <summary>True when the hold belongs to the user the map was built for.</summary>
    public bool HeldByMe { get; set; }
}
