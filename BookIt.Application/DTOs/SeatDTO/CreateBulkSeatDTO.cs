using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatDTO;

public class CreateBulkSeatDTO : IDTO
{
    public int HallId { get; set; }
    //public string HallName { get; set; } = null!;

    public int StartRow { get; set; }
    public int EndRow { get; set; }

    public int StartColumn { get; set; }
    public int EndColumn { get; set; }

    public int SeatTypeId { get; set; }
    //public string SeatName { get; set;} = null!;
}
