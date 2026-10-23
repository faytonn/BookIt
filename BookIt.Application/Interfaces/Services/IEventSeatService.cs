using BookIt.Application.DTOs.EventSeatDTO;

namespace BookIt.Application.Interfaces.Services;

public interface IEventSeatService
{
    Task<List<GetEventSeatDTO>> GetSeatMapAsync(int eventId, string? forUserId = null);
    Task<HoldResultDTO> HoldAsync(int eventId, int seatId, string userId);
    Task<HoldResultDTO> ConfirmAsync(int eventSeatId, string userId);
    Task<HoldResultDTO> ReleaseAsync(int eventSeatId, string userId);
    Task<int> ReleaseExpiredHoldsAsync();
    /// <summary>Creates the missing Available EventSeat rows for every seat in the event's hall.</summary>
    Task<int> EnsureSeatsForEventAsync(int eventId);
}
