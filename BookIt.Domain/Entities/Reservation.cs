using BookIt.Domain.Entities.Common;
using BookIt.Domain.Enums;

namespace BookIt.Domain.Entities;

public class Reservation : BaseAuditableEntity
{
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public int? PaymentTransactionId { get; set; }

    public int NumberOfTickets { get; set; }
    public DateTime ReservationDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ReservationStatus Status { get; set; }


    public CancellationRefund? CancellationRefund { get; set; }
    public Event? Event { get; set; }
    public ApplicationUser? User { get; set; }
    public PaymentTransaction? PaymentTransaction { get; set; }

}
