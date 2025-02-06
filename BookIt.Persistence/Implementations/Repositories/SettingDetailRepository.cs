using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class SettingDetailRepository : Repository<SettingDetail>, ISettingDetailRepository
{
    public SettingDetailRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
