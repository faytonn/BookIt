using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Seat : BaseEntity
{
    public string SeatName { get; set; } = null!;
    public int SeatColumn { get; set; }
    public int SeatRow { get; set;}

    public int SeatTypeId { get; set; }
    public SeatType SeatType { get; set; } = null!;

    //public bool IsReserved { get; set; } = false;

    public int HallId { get; set; }
    public Hall Hall { get; set; } = null!;

    public ICollection<ReservationSeat>? ReservationSeats { get; set; } = [];
}
