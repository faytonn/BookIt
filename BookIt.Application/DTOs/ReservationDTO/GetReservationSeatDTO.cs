using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ReservationDTO;

public class GetReservationSeatDTO : IDTO
{
    public int SeatId { get; set; }
    public string SeatName { get; set; } = null!;
    public int Row { get; set; }
    public int Column { get; set; }
    public decimal Price { get; set; }
}
