namespace BookIt.Application.DTOs.MessageDTO;

public class DisplayMessageDTO
{
    public string UserId { get; set; } = null!;
    public string ChatId { get; set; } = null!;

    public string? Fullname { get; set; }
    public string? ModeratorFullname { get; set; }
    public string? Text { get; set; }
    public DateTime SentAt { get; set; }
}
