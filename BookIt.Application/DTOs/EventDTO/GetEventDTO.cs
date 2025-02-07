using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventDTO;

public class GetEventDTO : IDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string PriceRange { get; set; } = null!;
    public bool IsSoldOut { get; set; }
    public int GeneralLocationId { get; set; }
    public int? CategoryId { get; set; }
}
