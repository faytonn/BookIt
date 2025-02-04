using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Review : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public int EventId { get; set; }
    public int Rating { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    public ApplicationUser? User { get; set; }
    public Event? Event { get; set; }

}
