namespace BookIt.Core.Domain.Entities
{
    public class DashboardStats
    {
        public int TotalUsers { get; set; }
        public int ActiveEvents { get; set; }
        public int TicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal UserGrowth { get; set; }
        public decimal EventGrowth { get; set; }
        public decimal TicketGrowth { get; set; }
        public decimal RevenueGrowth { get; set; }
    }

    public class RevenueData
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class EventTypeData
    {
        public string Type { get; set; }
        public int Count { get; set; }
    }

    public class TopEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardData
    {
        public DashboardStats Stats { get; set; }
        public List<RevenueData> RevenueData { get; set; }
        public List<EventTypeData> EventTypeData { get; set; }
        public List<TopEvent> TopEvents { get; set; }
    }
}
