using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Repositories;

public class SeatRepository : Repository<Seat>, ISeatRepository
{
    public SeatRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Seat>> GetByHallIdAsync(int hallId)
    {
        return await GetAll(s => s.HallId == hallId).ToListAsync();
    }

    public async Task<bool> IsSeatInEventHallAsync(int seatId, int eventId)
    {
        var seat = await GetAsync(seatId);
        if (seat == null) return false;

        return true;
    }
}
