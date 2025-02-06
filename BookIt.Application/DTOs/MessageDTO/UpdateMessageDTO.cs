using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.MessageDTO;

public class UpdateMessageDTO : IDTO
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public int ChatId { get; set; }
}
