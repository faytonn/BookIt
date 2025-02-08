using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class ReservationSeat : BaseAuditableEntity
{
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int SeatId { get; set; }
    public Seat? Seat { get; set; }

    public decimal Price { get; set; }
}
