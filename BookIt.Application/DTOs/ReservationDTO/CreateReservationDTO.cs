using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.ReservationDTO;

public class CreateReservationDTO : IDTO
{
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public int NumberOfTickets { get; set; }
    public DateTime ReservationDate { get; set; }
    public decimal TotalAmount { get; set; }
    //public ReservationStatus ReservationStatus { get; set; }
    public List<int> SeatIds { get; set; } = [];
}
