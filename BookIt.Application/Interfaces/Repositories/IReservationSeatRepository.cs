using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IReservationSeatRepository : IRepository<ReservationSeat>
{
    Task<List<ReservationSeat>> GetByReservationIdAsync(int reservationId);
    Task<List<int>> GetSeatIdsByReservationAsync(int reservationId);

    /// <summary>Sets CheckedInAt only if it is still empty. Returns 1 if this call admitted the ticket, 0 if it was already used.</summary>
    Task<int> CheckInAsync(int ticketId, DateTime checkedInAt, string checkedInBy);
}
