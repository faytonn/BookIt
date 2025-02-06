using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;

namespace BookIt.Persistence.Implementations.Repositories;

public class EventDetailRepository : Repository<EventDetail>, IEventDetailRepository
{
    public EventDetailRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}

