using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.ReservationDTO;

public class GetReservationDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ReservationStatus Status { get; set; }
    public int? PaymentTransactionId { get; set; }
    public int NumberOfTickets { get; set; }
    public DateTime ReservationDate { get; set; }
    public List<GetReservationSeatDTO> Seats { get; set; } = [];
}