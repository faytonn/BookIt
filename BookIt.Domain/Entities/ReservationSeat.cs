using BookIt.Domain.Entities.Common;
using BookIt.Domain.Helpers;

namespace BookIt.Domain.Entities;

/// <summary>One seat in a booking, and also the ticket for it: TicketCode is what the QR code holds.</summary>
public class ReservationSeat : BaseAuditableEntity
{
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int SeatId { get; set; }
    public Seat? Seat { get; set; }

    public decimal Price { get; set; }

    public string TicketCode { get; set; } = TicketCodes.NewTicketCode();
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
}
