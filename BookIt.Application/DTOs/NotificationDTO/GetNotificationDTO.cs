using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NotificationDTO;

public class GetNotificationDTO : IDTO
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
}

