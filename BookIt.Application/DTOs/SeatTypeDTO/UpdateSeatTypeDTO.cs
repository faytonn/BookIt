using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatTypeDTO;

public class UpdateSeatTypeDTO : IDTO
{
    public string Name { get; set; } = null!;
    public decimal DefaultPrice { get; set; }
    public string? Description { get; set; }
    public int HallId { get; set; }
}
