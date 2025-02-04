using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public Category? ParentCategory { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<Category> ChildCategories { get; set;} = [];
    public List<Event> Events { get; set; } = [];
}
