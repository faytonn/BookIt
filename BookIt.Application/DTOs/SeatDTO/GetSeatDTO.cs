using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatDTO;

public class GetSeatDTO : IDTO
{
    public int Id { get; set; }
    public string SeatName { get; set; } = string.Empty; //or null!;, idk yet
    public int SeatRow { get; set; }
    public int SeatColumn { get; set; }
    public int SeatTypeId { get; set; }
    public int HallId { get; set; }
    //public string HallName { get; set; }
    public bool IsReserved { get; set; }

    public string SeatTypeName { get; set; } = string.Empty; //or null!;, idk yet
}
