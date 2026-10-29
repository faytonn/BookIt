using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.DashboardDTO;

public class DashboardEventDTO : IDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ImagePath { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string LocationName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public int SeatsSold { get; set; }
    public int SeatsTotal { get; set; }

    public int PercentSold => SeatsTotal == 0 ? 0 : (int)Math.Round(100.0 * SeatsSold / SeatsTotal);
}
