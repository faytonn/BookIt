using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.MessageDTO;

public class CreateMessageDTO : IDTO
{
    public string? Text { get; set; }
    public int ChatId { get; set; }
}
