using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.TicketDTO;

public class GetTicketDTO : IDTO
{
    public string TicketCode { get; set; } = null!;
    public string SeatName { get; set; } = null!;
    public int Row { get; set; }
    public int Column { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
}
