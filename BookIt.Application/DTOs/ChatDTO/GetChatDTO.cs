using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.MessageDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.DTOs.ChatDTO;

public class GetChatDTO : IDTO
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public ApplicationUser? User { get; set; }
    public string? ModeratorId { get; set; }
    public ApplicationUser? Moderator { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GetMessageDTO> Messages { get; set; } = [];
}
