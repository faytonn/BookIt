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

    public async Task<List<ReservationSeat>> GetActiveBySeatAndEventAsync(int seatId, int eventId)
    {
        // 
        // checking if the reservation is not cancelled + assuming Reservation.Status != ReservationStatus.Cancelled
        // or something similar. Let's do an example:

        return await GetAll(
            rs => rs.SeatId == seatId
                  && rs.Reservation!.EventId == eventId
                  && rs.Reservation.Status != Domain.Enums.ReservationStatus.Cancelled)
            .Include(rs => rs.Reservation)  
            .ToListAsync();
    }

    public async Task<List<int>> GetSeatIdsByReservationAsync(int reservationId)
    {
        return await GetAll(rs => rs.ReservationId == reservationId)
            .Select(rs => rs.SeatId)
            .ToListAsync();
    }

    public async Task<List<ReservationSeat>> GetActiveByEventIdAsync(int eventId)
    {
        return await GetAll(
            rs => rs.Reservation!.EventId == eventId
                  && rs.Reservation.Status != Domain.Enums.ReservationStatus.Cancelled)
            .Include(rs => rs.Reservation)
            .ToListAsync();
    }
}
