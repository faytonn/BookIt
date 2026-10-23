using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.EventDTO;

public class EventFilterDTO : IDTO
{
    public string? SearchTerm { get; set; }
    public int? LocationId { get; set; }
    public int? CategoryId { get; set; }
    public DateTime? Date { get; set; }
    public int Page { get; set; } = 1;
}
