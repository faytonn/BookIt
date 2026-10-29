using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.DashboardDTO;

public class DashboardDayDTO : IDTO
{
    public DateTime Day { get; set; }
    public int Tickets { get; set; }
    public decimal Revenue { get; set; }
}
