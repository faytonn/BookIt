using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDTO;

public class UpdateNewsDTO : IDTO
{
    public string? ImagePath { get; set; }
}
