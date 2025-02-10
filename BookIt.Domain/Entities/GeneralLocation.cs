using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class GeneralLocation : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;

    public ICollection<Hall> Halls { get; set; } = [];
}
