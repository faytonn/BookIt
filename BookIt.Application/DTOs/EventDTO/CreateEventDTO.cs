using BookIt.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace BookIt.Application.DTOs.EventDTO;

public class CreateEventDTO : IDTO
{
    public string Title { get; set; } = null!;

    public IFormFile ImageFile { get; set; } = null!;
    public string ImagePath { get; set; } = null!;

    public DateTime EventDate { get; set; }
    public string PriceRange { get; set; } = null!;
    public bool IsSoldOut { get; set; }
    public int GeneralLocationId { get; set; }
    public int? ParentCategoryId { get; set; }
    public int CategoryId { get; set; }
}
