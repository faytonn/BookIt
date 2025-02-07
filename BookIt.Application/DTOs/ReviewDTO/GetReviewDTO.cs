using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.ReviewDTO;

public class GetReviewDTO : IDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int Rating { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}