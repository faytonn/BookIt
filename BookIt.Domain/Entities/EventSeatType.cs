using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class EventSeatType : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int SeatTypeId { get; set; }
    public SeatType SeatType { get; set; } = null!;

    public decimal Price { get; set; }

    public string? AdditionalDetails { get; set; }
}
