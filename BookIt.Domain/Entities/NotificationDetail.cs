using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class NotificationDetail : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int LanguageId { get; set; }
    public Language? Language { get; set; }
    public int NotificationId { get; set; }
    public Notification? Notification { get; set; }
}
