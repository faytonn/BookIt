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
    public string LocationName { get; set; } = null!;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    


    //public string GeneralLocationName { get; set; } = null!;


}
