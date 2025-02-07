using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class GetNotificationDetailDTO : IDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int LanguageId { get; set; }
    public int NotificationId { get; set; }
}
