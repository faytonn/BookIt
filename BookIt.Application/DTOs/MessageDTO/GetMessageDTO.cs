using BookIt.Application.DTOs.ChatDTO;
using BookIt.Application.DTOs.Common;
using BookIt.Domain.Entities;

namespace BookIt.Application.DTOs.MessageDTO;

public class GetMessageDTO : IDTO
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public int ChatId { get; set; }
    public GetChatDTO? Chat { get; set; }
    public DateTime CreatedAt { get; set; }
}
