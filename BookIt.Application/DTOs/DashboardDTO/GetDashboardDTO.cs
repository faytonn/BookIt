using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.DashboardDTO;

public class GetDashboardDTO : IDTO
{
    public int UpcomingEvents { get; set; }
    public int SoldOutEvents { get; set; }
    public int TicketsSold { get; set; }
    public int SeatsOnSale { get; set; }
    public decimal Revenue { get; set; }
    public decimal RevenueLast7Days { get; set; }
    public int ConfirmedReservations { get; set; }
    public int ActiveHolds { get; set; }
    public List<DashboardEventDTO> NextEvents { get; set; } = [];
    public List<DashboardCategoryDTO> SalesByCategory { get; set; } = [];
    public List<DashboardDayDTO> SalesByDay { get; set; } = [];
    public List<DashboardReservationDTO> RecentReservations { get; set; } = [];
}
