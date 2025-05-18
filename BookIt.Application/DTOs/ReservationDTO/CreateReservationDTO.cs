using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ReservationDTO;

public class CreateReservationDTO : IDTO
{
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public List<int> SeatIds { get; set; } = [];
}
