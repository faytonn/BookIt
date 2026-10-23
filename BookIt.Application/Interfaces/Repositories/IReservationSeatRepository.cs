using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IReservationSeatRepository : IRepository<ReservationSeat>
{
    Task<List<ReservationSeat>> GetByReservationIdAsync(int reservationId);
    Task<List<int>> GetSeatIdsByReservationAsync(int reservationId);
}
