using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class CancellationRefund : BaseAuditableEntity
{
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public DateTime CancellationDate { get; set; }
    public decimal RefundAmount { get; set; }

    public bool IsRefunded { get; set; }

}
