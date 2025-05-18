using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.ReservationDTO;

public class UpdateReservationDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public List<int> SeatIds { get; set; } = [];
    public ReservationStatus Status { get; set; }
}
