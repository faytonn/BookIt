using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class BaseEmailNotificationDTO : IDTO
{
    public string Title { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
