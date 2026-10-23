using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Repositories;

public class ReservationSeatRepository : Repository<ReservationSeat>, IReservationSeatRepository
{
    public ReservationSeatRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<ReservationSeat>> GetByReservationIdAsync(int reservationId)
    {
        return await GetAll(rs => rs.ReservationId == reservationId)
            .ToListAsync();
    }


    public async Task<List<int>> GetSeatIdsByReservationAsync(int reservationId)
    {
        return await GetAll(rs => rs.ReservationId == reservationId)
            .Select(rs => rs.SeatId)
            .ToListAsync();
    }

}
