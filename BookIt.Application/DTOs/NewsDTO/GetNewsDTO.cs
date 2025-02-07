using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDTO;

public class GetNewsDTO : IDTO
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set;} = DateTime.MinValue;
}


