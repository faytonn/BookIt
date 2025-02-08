using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class CategoryDetailRepository : Repository<CategoryDetail>, ICategoryDetailRepository
{
    public CategoryDetailRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
