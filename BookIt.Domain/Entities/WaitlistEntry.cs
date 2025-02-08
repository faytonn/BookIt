using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class WaitlistEntry : BaseAuditableEntity
{
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
    public bool IsNotified { get; set; } = false;

    public Event? Event { get; set; }
    public ApplicationUser? User { get; set; }
}
