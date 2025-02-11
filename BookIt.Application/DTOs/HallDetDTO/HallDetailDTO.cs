using BookIt.Application.DTOs.Common;

public class HallDetailDTO : IDTO
{
    public int LanguageId { get; set; }
    public string Name { get; set; } = null!;
}
