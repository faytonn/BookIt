using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class SeatType : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public decimal DefaultPrice { get; set; }
    public string? Description { get; set; }

    public int? HallId { get; set; }
    public Hall? Hall { get; set; }

    //public ICollection</*EventDetailSeatType*/> EventSeatTypes { get; set; } = [];


}
