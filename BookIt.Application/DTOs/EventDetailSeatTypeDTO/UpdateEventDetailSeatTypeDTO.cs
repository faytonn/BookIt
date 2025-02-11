using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventSeatTypeDTO;

public class UpdateEventDetailSeatTypeDTO : IDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string? AdditionalDetails { get; set; }
}