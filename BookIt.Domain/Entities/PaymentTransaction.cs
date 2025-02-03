using BookIt.Domain.Entities.Common;
using BookIt.Domain.Enums;

namespace BookIt.Domain.Entities;

public class PaymentTransaction :BaseAuditableEntity
{
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public string TransactionReference { get; set; }
    public Reservation Reservation { get; set; }
}
