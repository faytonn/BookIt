using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.DTOs.GeneralLocationDTO;
using BookIt.Application.DTOs.HallDTO;

namespace BookIt.Application.DTOs.NotificationDetailDTO;

public class CreateSuccessfulPurchaseNotificationDTO : BaseEmailNotificationDTO
{
    public GetEventDTO EventInfo { get; set; } = null!;
    public GetGeneralLocationDTO EventLocation { get; set; } = null!; // E.g., the venue or general location.
    public GetHallDTO Hall { get; set; } = null!; // Specific hall name if applicable.
    public int TicketCount { get; set; }
    public decimal TotalPrice { get; set; }
    public string? TicketDetails { get; set; } // Optional summary, e.g. "VIP Section, Row 3, Seat 15"
    public string? AdditionalInstructions { get; set; } // E.g., "Please arrive 30 minutes early."
}
