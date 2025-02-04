using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Chat : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public string? ModeratorId { get; set; }
    public ApplicationUser? Moderator { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Message> Messages { get; set; } = [];
}
