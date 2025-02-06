using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SettingDTO;

public class GetSettingDTO : IDTO
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
