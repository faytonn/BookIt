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

    /// <summary>
    /// Conditional UPDATE ... WHERE CheckedInAt IS NULL: when two doors scan the same ticket at once,
    /// the database lets exactly one of them change the row, the other gets 0 back.
    /// </summary>
    public async Task<int> CheckInAsync(int ticketId, DateTime checkedInAt, string checkedInBy)
    {
        return await GetAll(rs => rs.Id == ticketId && rs.CheckedInAt == null)
            .ExecuteUpdateAsync(set => set
                .SetProperty(rs => rs.CheckedInAt, checkedInAt)
                .SetProperty(rs => rs.CheckedInBy, checkedInBy));
    }
}
