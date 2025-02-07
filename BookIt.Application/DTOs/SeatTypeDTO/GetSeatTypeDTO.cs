using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatTypeDTO;

public class GetSeatTypeDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal DefaultPrice { get; set; }
    public string? Description { get; set; }
    public int HallId { get; set; }
}