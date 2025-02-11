using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventSeatTypeDTO;

public class GetEventDetailSeatTypeDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int SeatTypeId { get; set; }
    public decimal Price { get; set; }
    public string? AdditionalDetails { get; set; }
}
