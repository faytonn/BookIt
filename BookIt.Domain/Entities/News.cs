using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class News : BaseAuditableEntity
{
    public string? ImagePath {  get; set; }
    public ICollection<NewsDetail> NewsDetails { get; set; } = [];
}
