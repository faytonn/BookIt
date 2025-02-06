using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.SettingDetail;

namespace BookIt.Application.DTOs.SettingDTO;

public class UpdateSettingDTO : IDTO
{
    public int Id { get; set; }
    public string? Key { get; set; }
    public List<UpdateSettingDetailDTO> SettingDetails { get; set; } = [];
}
