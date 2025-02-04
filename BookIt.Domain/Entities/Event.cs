using BookIt.Domain.Entities.Common;
using System.Data.Common;

namespace BookIt.Domain.Entities;

public class Event : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
    public ICollection<EventImage> Images { get; set; } = new List<EventImage>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

}
