using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class GeneralLocationRepository : Repository<GeneralLocation>, IGeneralLocationRepository
{
    public GeneralLocationRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
