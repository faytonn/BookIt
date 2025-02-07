using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDetailsDTO;

public class CreateNewsDetailDTO : IDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int NewsId { get; set; }
    public int LanguageId { get; set; }
}
