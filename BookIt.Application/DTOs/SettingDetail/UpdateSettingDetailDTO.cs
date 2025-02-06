namespace BookIt.Application.DTOs.SettingDetail;

public class UpdateSettingDetailDTO
{
    public int Id { get; set; }
    public string? Value { get; set; }
    public int LanguageId { get; set; }
    public int SettingId { get; set; }
}
