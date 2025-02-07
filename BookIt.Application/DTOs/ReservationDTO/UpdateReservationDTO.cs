using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.ReservationDTO;

public class UpdateReservationDTO : IDTO
{
    public int NumberOfTickets { get; set; }
    public DateTime ReservationDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
}
