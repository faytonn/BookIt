using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.NewsDTO;

public class CreateNewsDTO : IDTO
{
    public string? ImagePath { get; set; }
}
