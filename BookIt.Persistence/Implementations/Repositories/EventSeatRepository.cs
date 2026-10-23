using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Repositories;

public class EventSeatRepository : Repository<EventSeat>, IEventSeatRepository
{
    public EventSeatRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<EventSeat>> GetByEventAsync(int eventId)
    {
        return await GetAll(es => es.EventId == eventId, include: q => q.Include(es => es.Seat).ThenInclude(s => s.SeatType))
            .OrderBy(es => es.Seat.SeatRow)
            .ThenBy(es => es.Seat.SeatColumn)
            .ToListAsync();
    }

    public async Task<EventSeat?> GetByEventAndSeatAsync(int eventId, int seatId)
    {
        return await GetAsync(es => es.EventId == eventId && es.SeatId == seatId,
            include: q => q.Include(es => es.Seat).ThenInclude(s => s.SeatType));
    }

    /// <summary>
    /// Set-based release of expired holds; runs as one UPDATE without loading rows,
    /// so the background sweep and a concurrent hold never fight over tracked entities.
    /// </summary>
    public async Task<int> ReleaseExpiredHoldsAsync(DateTime utcNow)
    {
        return await GetAll(es => es.Status == EventSeatStatus.Held && es.HeldUntil != null && es.HeldUntil < utcNow)
            .ExecuteUpdateAsync(set => set
                .SetProperty(es => es.Status, EventSeatStatus.Available)
                .SetProperty(es => es.HeldUntil, (DateTime?)null)
                .SetProperty(es => es.HeldByUserId, (string?)null));
    }
}
