using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.MessageDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.DTOs.ChatDTO;

internal class UpdateChatDTO : IDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public string ModeratorId { get; set; } = null!;
    public ApplicationUser? Moderator { get; set; }
    public bool IsActive { get; set; }
    public List<GetMessageDTO> Messages { get; set; } = [];
}
