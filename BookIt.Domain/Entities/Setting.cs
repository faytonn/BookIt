using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Setting : BaseEntity
{
    //public Setting();

    public string? Key { get; set; }
    public ICollection<SettingDetail>? SettingDetails { get; set; }
}
