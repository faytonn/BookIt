using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Language : BaseEntity
{
    public string LangaugeName { get; set; } = null!;
    public string IsoCode { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
}
