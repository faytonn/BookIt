using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IEventSeatRepository : IRepository<EventSeat>
{
    Task<List<EventSeat>> GetByEventAsync(int eventId);
    Task<EventSeat?> GetByEventAndSeatAsync(int eventId, int seatId);
    Task<int> ReleaseExpiredHoldsAsync(DateTime utcNow);
}
