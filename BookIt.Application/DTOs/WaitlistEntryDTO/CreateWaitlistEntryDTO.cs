using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.WaitlistEntryDTO;

public class CreateWaitlistEntryDTO : IDTO
{
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime RequestedDate { get; set; }
}
