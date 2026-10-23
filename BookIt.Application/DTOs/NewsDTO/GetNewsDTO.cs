using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDTO;

public class GetNewsDTO : IDTO
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
