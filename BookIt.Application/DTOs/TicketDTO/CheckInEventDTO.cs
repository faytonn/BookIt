using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.TicketDTO;

public class CheckInEventDTO : IDTO
{
    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public int TicketCount { get; set; }
    public int CheckedInCount { get; set; }
}
