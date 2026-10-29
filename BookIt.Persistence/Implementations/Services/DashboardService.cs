using BookIt.Application.DTOs.DashboardDTO;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

/// <summary>
/// Read-only numbers for the admin dashboard. Every figure is one aggregate query; nothing is cached.
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventSeatRepository _eventSeatRepository;
    private readonly IReservationRepository _reservationRepository;

    public DashboardService(IEventRepository eventRepository, IEventSeatRepository eventSeatRepository, IReservationRepository reservationRepository)
    {
        _eventRepository = eventRepository;
        _eventSeatRepository = eventSeatRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<GetDashboardDTO> GetAsync(int salesDays = 14)
    {
        var now = DateTime.UtcNow;
        var upcoming = _eventRepository.GetAll(e => e.EventDate >= now);
        var confirmed = _reservationRepository.GetAll(r => r.Status == ReservationStatus.Confirmed);
        var upcomingSeats = _eventSeatRepository.GetAll(s => s.Event.EventDate >= now && !s.Event.IsDeleted);

        var dto = new GetDashboardDTO
        {
            UpcomingEvents = await upcoming.CountAsync(),
            SoldOutEvents = await upcoming.CountAsync(e => e.IsSoldOut),
            TicketsSold = await _eventSeatRepository.GetAll(s => s.Status == EventSeatStatus.Sold).CountAsync(),
            SeatsOnSale = await upcomingSeats.CountAsync(),
            Revenue = await confirmed.SumAsync(r => r.TotalAmount),
            RevenueLast7Days = await confirmed.Where(r => r.ReservationDate >= now.AddDays(-7)).SumAsync(r => r.TotalAmount),
            ConfirmedReservations = await confirmed.CountAsync(),
            // expired holds count as free (see EventSeatService), so only live ones are shown
            ActiveHolds = await _eventSeatRepository.GetAll(s => s.Status == EventSeatStatus.Held && s.HeldUntil > now).CountAsync()
        };

        dto.NextEvents = await upcoming
            .OrderBy(e => e.EventDate)
            .Take(6)
            .Select(e => new DashboardEventDTO
            {
                Id = e.Id,
                Title = e.Title,
                ImagePath = e.ImagePath,
                EventDate = e.EventDate,
                LocationName = e.GeneralLocation.Name,
                CategoryName = e.Category.Name,
                SeatsSold = e.EventSeats.Count(s => s.Status == EventSeatStatus.Sold),
                SeatsTotal = e.EventSeats.Count()
            })
            .ToListAsync();

        dto.SalesByCategory = await _eventSeatRepository.GetAll(s => s.Status == EventSeatStatus.Sold)
            .GroupBy(s => s.Event.Category.Name)
            .Select(g => new DashboardCategoryDTO { Name = g.Key, TicketsSold = g.Count() })
            .OrderByDescending(c => c.TicketsSold)
            .ToListAsync();

        var from = now.Date.AddDays(1 - salesDays);
        var perDay = await confirmed
            .Where(r => r.ReservationDate >= from)
            .GroupBy(r => r.ReservationDate.Date)
            .Select(g => new DashboardDayDTO { Day = g.Key, Tickets = g.Sum(r => r.NumberOfTickets), Revenue = g.Sum(r => r.TotalAmount) })
            .ToListAsync();
        // one entry per day, including days without sales, so the chart has no gaps
        dto.SalesByDay = Enumerable.Range(0, salesDays)
            .Select(i => from.AddDays(i))
            .Select(day => perDay.FirstOrDefault(d => d.Day == day) ?? new DashboardDayDTO { Day = day })
            .ToList();

        dto.RecentReservations = await _reservationRepository.GetAll()
            .OrderByDescending(r => r.ReservationDate)
            .Take(6)
            .Select(r => new DashboardReservationDTO
            {
                Id = r.Id,
                EventTitle = r.Event!.Title,
                UserEmail = r.User!.Email ?? string.Empty,
                NumberOfTickets = r.NumberOfTickets,
                TotalAmount = r.TotalAmount,
                ReservationDate = r.ReservationDate,
                Status = r.Status
            })
            .ToListAsync();

        return dto;
    }
}
