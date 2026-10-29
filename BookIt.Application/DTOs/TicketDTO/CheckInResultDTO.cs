using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.TicketDTO;

public enum CheckInOutcome
{
    Admitted,
    /// <summary>The ticket was already scanned; nobody gets in twice on one ticket.</summary>
    AlreadyUsed,
    Unknown,
    /// <summary>A real ticket, but for another event.</summary>
    WrongEvent,
    /// <summary>The booking is cancelled, refunded or not confirmed.</summary>
    NotValid
}

public class CheckInResultDTO : IDTO
{
    public CheckInOutcome Outcome { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? BookingCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SeatName { get; set; }
    public string? SeatType { get; set; }
    public string? EventTitle { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
    public int CheckedInCount { get; set; }
    public int TicketCount { get; set; }

    public bool Admitted => Outcome == CheckInOutcome.Admitted;
}
