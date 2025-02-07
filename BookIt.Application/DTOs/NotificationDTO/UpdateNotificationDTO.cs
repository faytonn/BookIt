using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.NotificationDetailDTO;

namespace BookIt.Application.DTOs.NotificationDTO;

public class UpdateNotificationDTO : IDTO
{
    public int Id { get; set; }
    public DateTime? SentTime { get; set; }
    public List<UpdateNotificationDetailDTO> NotificationDetails { get; set; } = [];
}
