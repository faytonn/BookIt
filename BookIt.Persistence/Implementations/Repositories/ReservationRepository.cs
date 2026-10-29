using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    public async Task<Reservation?> GetWithDetailsAsync(int id)
    {
        return await GetAsync(r => r.Id == id, include: _withDetails());
    }

    public List<Reservation> GetAllWithDetails()
    {
        return GetAll(include: _withDetails())
            .OrderByDescending(r => r.ReservationDate)
            .ToList();
    }

    public async Task<List<Reservation>> GetPageAsync(int page, int limit)
    {
        var query = GetAll(include: _withDetails()).OrderByDescending(r => r.ReservationDate);
        return await Paginate(query, limit, page).ToListAsync();
    }

    public async Task<List<Reservation>> GetByUserAsync(string userId)
    {
        return await GetAll(r => r.UserId == userId, include: _withDetails())
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
    }

    private static Func<IQueryable<Reservation>, IIncludableQueryable<Reservation, object>> _withDetails()
    {
        return q => q.Include(r => r.User)
                     .Include(r => r.CancellationRefund)
                     .Include(r => r.Event).ThenInclude(e => e!.GeneralLocation)
                     .Include(r => r.Event).ThenInclude(e => e!.Hall)
                     .Include(r => r.ReservationSeats)!
                     .ThenInclude(rs => rs.Seat)!
                     .ThenInclude(s => s!.SeatType);
    }
}
