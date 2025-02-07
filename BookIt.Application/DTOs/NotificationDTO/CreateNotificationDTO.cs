using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.NotificationDetailDTO;

namespace BookIt.Application.DTOs.NotificationDTO;

public class CreateNotificationDTO : IDTO
{
    public DateTime? SentTime { get; set; }
    public List<CreateSubscriptionNotificationDetailDTO> NotificationDetails { get; set; } = [];
}
