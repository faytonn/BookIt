using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    //public bool IsDeleted { get; set; }

    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public ICollection<Category> ChildCategories { get; set;} = [];
    public ICollection<CategoryDetail> CategoryDetails { get; set; } = [];
    public ICollection<Event> Events { get; set; } = [];
}
