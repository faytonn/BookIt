using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class SeatTypeRepository : Repository<SeatType>, ISeatTypeRepository
{
    public SeatTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
