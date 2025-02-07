using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.WaitlistEntryDTO;

public class UpdateWaitlistEntryDTO : IDTO
{
    public DateTime RequestedDate { get; set; }
    public bool IsNotified { get; set; }
}
