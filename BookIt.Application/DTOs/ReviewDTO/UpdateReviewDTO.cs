using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ReviewDTO;

public class UpdateReviewDTO : IDTO
{
    public int Rating { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
}
