using BookIt.Application.DTOs.TicketDTO;

namespace BookIt.Application.Interfaces.Services;

public interface ITicketService
{
    /// <summary>A booking and its tickets by booking code ("BK-..."), or null. The caller checks ownership.</summary>
    Task<GetBookingTicketsDTO?> GetBookingAsync(string bookingCode);

    /// <summary>Owner of the ticket with this code, or null if there is no such ticket.</summary>
    Task<string?> GetTicketOwnerAsync(string ticketCode);

    /// <summary>Events door staff can scan for: from a few hours ago up to the next days, with counts.</summary>
    Task<List<CheckInEventDTO>> GetCheckInEventsAsync();

    /// <summary>Admits a ticket once. Safe when two scanners read the same code at the same moment.</summary>
    Task<CheckInResultDTO> CheckInAsync(string ticketCode, int eventId, string staffName);
}
