using BookIt.Domain.Entities.Common;
using System.Data.Common;

namespace BookIt.Domain.Entities;

public class Event : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string ImagePath { get; set; } = string.Empty;
    //public string Description { get; set; } = null!;
    public int LanguageId { get; set; }
    public DateTime EventDate { get; set; }
    public string PriceRange { get; set; } = null!;
    public bool IsSoldOut { get; set; } = false;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int GeneralLocationId { get; set; }
    public GeneralLocation GeneralLocation { get; set; } = null!;
    //public int HallId { get; set; }
    //public Hall Hall { get; set; } = null!;
    //public int TotalSeats { get; set; }
    //public int AvailableSeats { get; set; }
    //public decimal Price { get; set; }
    //public bool isSoldOut { get; set; }
    public ICollection<EventDetail> EventDetail { get; set; } = [];
    public ICollection<Reservation> Reservations { get; set; } = [];
    public ICollection<WaitlistEntry>? WaitlistEntries { get; set; }

}
