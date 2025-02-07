using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.WaitlistEntryDTO;

public class GetWaitlistEntryDTO : IDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime RequestedDate { get; set; }
    public bool IsNotified { get; set; }
}