using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<int> CountAsync();
    Task<List<Reservation>> GetPageAsync(int page, int limit);

    Task<List<Reservation>> GetByUserAsync(string userId);

}
