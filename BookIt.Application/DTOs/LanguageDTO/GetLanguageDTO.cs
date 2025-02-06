using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.LanguageDTO;

public class GetLanguageDTO : IDTO
{
    public int Id { get; set; }
    public string? IsoCode { get; set; }
    public string? ImagePath { get; set; }
}
