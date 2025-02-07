namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class CreateEmailVerificationNotificationDTO : BaseEmailNotificationDTO
{
    public string VerificationLink { get; set; } = null!;
}
