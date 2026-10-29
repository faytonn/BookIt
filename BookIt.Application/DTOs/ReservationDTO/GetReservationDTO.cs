using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.ReservationDTO;

public class GetReservationDTO : IDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string EventTitle { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ReservationStatus Status { get; set; }
    public int? PaymentTransactionId { get; set; }
    public int NumberOfTickets { get; set; }
    public DateTime ReservationDate { get; set; }
    public List<GetReservationSeatDTO> Seats { get; set; } = [];

    // Customer
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerAddress { get; set; }
    public bool CustomerEmailConfirmed { get; set; }
    public DateTime? CustomerSince { get; set; }

    // Event
    public DateTime EventDate { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;

    // Refund (set when a paid booking is cancelled)
    public decimal? RefundAmount { get; set; }
    public bool IsRefunded { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Audit (filled by BaseEntityInterceptor)
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}