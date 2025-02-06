using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class SettingDetail : BaseEntity
{
    //public SettingDetail() { }

    public string Value { get; set; } = null!;
    public int SettingId { get; set; }
    public Setting? Setting { get; set; }
    public int LanguageId { get; set; }
    public Language? Language { get; set; }
}
