using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Slider : BaseEntity
{
    public string ImagePath { get; set; } = null!;
}
