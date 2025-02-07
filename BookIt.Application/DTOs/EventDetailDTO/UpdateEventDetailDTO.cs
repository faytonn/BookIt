using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventDetailDTO;

public class UpdateEventDetailDTO : IDTO
{
    public string Title { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public int LocationId { get; set; }
    public string Description { get; set; } = null!;
    public string PriceRange { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int HallId { get; set; }
    public decimal Price { get; set; }
    public bool IsSoldOut { get; set; }
}
