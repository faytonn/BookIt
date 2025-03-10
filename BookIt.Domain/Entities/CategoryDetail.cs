﻿using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class CategoryDetail : BaseEntity
{
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int LanguageId { get; set; }
    public Language Language { get; set; } = null!;


}
