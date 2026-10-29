using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.DashboardDTO;

public class DashboardCategoryDTO : IDTO
{
    public string Name { get; set; } = null!;
    public int TicketsSold { get; set; }
}
