using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ChatDTO;

public class CreateChatDTO : IDTO
{
    public int UserId { get; set; } 
    public int ModeratorId { get; set; }
}
