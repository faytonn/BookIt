using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class EventDetail : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public int LanguageId { get; set; }
    //public int LocationId { get; set; }
    //public GeneralLocation Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
    public bool IsSoldOut { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

}
