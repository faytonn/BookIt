using System.Globalization;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.Implementations.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly AppDbContext _dbContext;

    public EventRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Event.IsSoldOut, Event.PriceRange and the EventDetail counts are copies of what EventSeats already says.
    /// They stay as columns (list pages read them without touching EventSeats), but only this method writes them.
    /// Set-based UPDATEs, so it never fights entities the caller is tracking.
    /// </summary>
    public async Task RefreshSeatStatsAsync(int eventId)
    {
        var seats = _dbContext.EventSeats.Where(s => s.EventId == eventId);
        var total = await seats.CountAsync();
        var sold = await seats.CountAsync(s => s.Status == EventSeatStatus.Sold);
        var prices = await seats.Select(s => s.Seat.SeatType.DefaultPrice).Distinct().ToListAsync();
        var soldOut = total > 0 && sold == total;
        var minPrice = prices.Count == 0 ? 0m : prices.Min();

        var events = _dbContext.Events.IgnoreQueryFilters().Where(e => e.Id == eventId);
        if (prices.Count == 0)
        {
            // no seats yet: keep whatever price range the admin typed
            await events.ExecuteUpdateAsync(set => set.SetProperty(e => e.IsSoldOut, false));
        }
        else
        {
            var range = string.Create(CultureInfo.InvariantCulture, $"{minPrice:0.00}-{prices.Max():0.00}");
            await events.ExecuteUpdateAsync(set => set
                .SetProperty(e => e.IsSoldOut, soldOut)
                .SetProperty(e => e.PriceRange, range));
        }

        await _dbContext.EventDetails.IgnoreQueryFilters()
            .Where(d => d.EventId == eventId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(d => d.TotalSeats, total)
                .SetProperty(d => d.AvailableSeats, total - sold)
                .SetProperty(d => d.IsSoldOut, soldOut)
                .SetProperty(d => d.Price, minPrice));
    }
}
