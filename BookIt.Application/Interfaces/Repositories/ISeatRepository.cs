using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface ISeatRepository : IRepository<Seat>
{
    Task<List<Seat>> GetByHallIdAsync(int hallId);
    Task<bool> IsSeatInEventHallAsync(int seatId, int eventId);
}
