using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class UserPositionDetail : BaseEntity
{
    public string Name { get; set; } = null!;
    public int UserPositionId { get; set; }
    public UserPosition? UserPosition { get; set; }
    public int LanguageId { get; set; }
    public Language? Language { get; set; }
}
