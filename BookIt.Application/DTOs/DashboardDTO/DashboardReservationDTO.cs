using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.DashboardDTO;

public class DashboardReservationDTO : IDTO
{
    public int Id { get; set; }
    public string EventTitle { get; set; } = null!;
    public string UserEmail { get; set; } = string.Empty;
    public int NumberOfTickets { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
}
