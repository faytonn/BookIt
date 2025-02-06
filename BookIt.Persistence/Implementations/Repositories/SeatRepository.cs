using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class SeatRepository : Repository<Seat>, ISeatRepository
{
    public SeatRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
