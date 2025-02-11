using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventSeatTypeDTO;

public class CreateEventDetailSeatTypeDTO : IDTO
{
    public int EventDetailId { get; set; }
    public int SeatTypeId { get; set; }
    public decimal Price { get; set; }
    public string? AdditionalDetails { get; set; }
}
