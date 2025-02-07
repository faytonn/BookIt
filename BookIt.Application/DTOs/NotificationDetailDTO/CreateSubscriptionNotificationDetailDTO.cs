using BookIt.Domain.Entities;

namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class CreateSubscriptionNotificationDetailDTO : BaseEmailNotificationDTO
{
    public DateOnly EventDate { get; set; }
    public TimeOnly EventTime { get; set; }
    public GeneralLocation GeneralLocation { get; set; } = null!;
    public string Description { get; set; } = null!;
}
