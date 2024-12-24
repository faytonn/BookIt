using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.Application.DTOs
{
    public class ReportRequestDTO
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string ReportType { get; set; } // sales, events, reservations
    }

    public class SalesReportDTO
    {
        public decimal TotalRevenue { get; set; }
        public int TotalTicketsSold { get; set; }
        public List<SalesReportItemDTO> SalesByEvent { get; set; } = new();
        public List<SalesReportItemDTO> SalesByMonth { get; set; } = new();
    }

    public class SalesReportItemDTO
    {
        public string Name { get; set; } // Event name or month name
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageTicketPrice => TicketsSold > 0 ? Revenue / TicketsSold : 0;
    }

    public class EventsReportDTO
    {
        public int TotalEvents { get; set; }
        public Dictionary<string, int> EventsByType { get; set; } = new();
        public Dictionary<string, int> EventsByCountry { get; set; } = new();
        public Dictionary<string, int> EventsByMonth { get; set; } = new();
        public Dictionary<string, decimal> AttendanceByType { get; set; } = new();
        public List<EventReportItemDTO> Events { get; set; } = new();
    }

    public class EventReportItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string LocationCity { get; set; } = string.Empty;
        public string LocationCountry { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats { get; set; }
        public decimal OccupancyRate => TotalSeats > 0 ? (decimal)SoldSeats / TotalSeats * 100 : 0;
    }

    public class ReservationsReportDTO
    {
        public int TotalReservations { get; set; }
        public Dictionary<string, int> ReservationsByStatus { get; set; } = new();
        public List<ReservationReportItemDTO> Reservations { get; set; } = new();
    }

    public class ReservationReportItemDTO
    {
        public string ReservationNumber { get; set; }
        public string EventTitle { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
