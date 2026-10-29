using BookIt.Application.Interfaces.Repositories.Generic;
using BookIt.Domain.Entities;

namespace BookIt.Application.Interfaces.Repositories;

public interface IEventRepository : IRepository<Event>
{
    /// <summary>
    /// Recomputes the cached seat numbers (Event.IsSoldOut, Event.PriceRange, EventDetail seat counts and price)
    /// from the EventSeat rows. Call it after anything that sells or frees seats.
    /// </summary>
    Task RefreshSeatStatsAsync(int eventId);
}
