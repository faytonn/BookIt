using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class EventImage : BaseEntity
{
    public int EventId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Caption { get; set; }

    public Event? Event { get; set; }

}
