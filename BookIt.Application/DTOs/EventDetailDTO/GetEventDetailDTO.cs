using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventDetailDTO;

public class GetEventDetailDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; } = null!;
    public int LocationId { get; set; }
    public string Description { get; set; } = null!;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
    public bool IsSoldOut { get; set; }
}
