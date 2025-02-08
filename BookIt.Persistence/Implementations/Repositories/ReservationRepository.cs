using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Repositories;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<int> CountAsync()
    {
        return await GetAll().CountAsync();
    }

    public async Task<List<Reservation>> GetPageAsync(int page, int limit)
    {
        return await Paginate(GetAll(), limit, page).ToListAsync();
    }

    public async Task<List<Reservation>> GetByUserAsync(string userId)
    {
        return await GetAll(r => r.UserId == userId)
            .ToListAsync();
    }
}
