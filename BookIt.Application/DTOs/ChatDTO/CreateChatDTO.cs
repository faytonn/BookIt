using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ChatDTO;

public class CreateChatDTO : IDTO
{
    public string UserId { get; set; } = null!;
    public string? ModeratorId { get; set; }
    public bool IsActive { get; set; } = true;
}
