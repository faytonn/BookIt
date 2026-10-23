using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<int> CountAsync();
    Task<Reservation?> GetWithDetailsAsync(int id);
    List<Reservation> GetAllWithDetails();
    Task<List<Reservation>> GetPageAsync(int page, int limit);
    Task<List<Reservation>> GetByUserAsync(string userId);
}
