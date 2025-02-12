using BookIt.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace BookIt.Application.DTOs.EventDetailDTO;

public class UpdateEventDetailDTO : IDTO
{
    public int Id { get; set; }

    public int EventId { get; set; }    
    public int LanguageId {  get; set; }
    public string Title { get; set; } = null!;

    public IFormFile? FormFile { get; set; } 
    public string ImagePath { get; set; } = string.Empty;

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
