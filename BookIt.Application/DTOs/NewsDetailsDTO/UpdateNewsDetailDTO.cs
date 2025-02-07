using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDetailsDTO;

public class UpdateNewsDetailDTO : IDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}