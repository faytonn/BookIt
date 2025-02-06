using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class NewsDetail : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int NewsId { get; set; }
    public News? News { get; set; }
    public int LanguageId { get; set; }
    public Language? Language { get; set; }
}
