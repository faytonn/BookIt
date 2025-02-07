using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.UserPositionDetailDTO;

public class GetUserPositionDetailDTO : IDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int LanguageId { get; set; }
}
