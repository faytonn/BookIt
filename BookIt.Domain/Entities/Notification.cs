using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Notification : BaseAuditableEntity
{
    public ICollection<NotificationDetail> NotificationDetails { get; set; } = [];
}
