using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class UpdateNotificationDetailDTO : IDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}