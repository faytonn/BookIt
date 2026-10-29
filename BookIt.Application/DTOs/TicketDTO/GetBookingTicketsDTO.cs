using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.TicketDTO;

public class GetBookingTicketsDTO : IDTO
{
    public int ReservationId { get; set; }
    public string BookingCode { get; set; } = null!;
    public ReservationStatus Status { get; set; }
    public string UserId { get; set; } = null!;
    public string CustomerName { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string EventTitle { get; set; } = null!;
    public string ImagePath { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public List<GetTicketDTO> Tickets { get; set; } = [];

    public bool IsValid => Status == ReservationStatus.Confirmed;
}
