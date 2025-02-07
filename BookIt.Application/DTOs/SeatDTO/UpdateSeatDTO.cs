using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatDTO;

public class UpdateSeatDTO : IDTO
{
    public string SeatName { get; set; } = null!;
    public int SeatRow { get; set; }
    public int SeatColumn { get; set; }
    public int SeatTypeId { get; set; }
    public int HallId { get; set; }
}
