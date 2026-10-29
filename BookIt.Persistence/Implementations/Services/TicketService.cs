using System.Globalization;
using BookIt.Application.DTOs.TicketDTO;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

/// <summary>
/// Tickets are ReservationSeat rows: one per seat, each with a secret TicketCode that the QR code holds.
/// Check-in admits a ticket once, for its own event, while its booking is Confirmed.
/// </summary>
public class TicketService : ITicketService
{
    // door staff see events from a few hours after their start (late arrivals) up to this far ahead
    private static readonly TimeSpan _checkInLookBack = TimeSpan.FromHours(12);
    private static readonly TimeSpan _checkInLookAhead = TimeSpan.FromDays(60);

    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationSeatRepository _reservationSeatRepository;
    private readonly IEventRepository _eventRepository;

    public TicketService(IReservationRepository reservationRepository, IReservationSeatRepository reservationSeatRepository, IEventRepository eventRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationSeatRepository = reservationSeatRepository;
        _eventRepository = eventRepository;
    }

    public async Task<GetBookingTicketsDTO?> GetBookingAsync(string bookingCode)
    {
        var code = bookingCode.Trim().ToUpperInvariant();
        var r = await _reservationRepository.GetAsync(x => x.Code == code,
            include: q => q.Include(x => x.User)
                .Include(x => x.Event).ThenInclude(e => e!.GeneralLocation)
                .Include(x => x.Event).ThenInclude(e => e!.Hall)
                .Include(x => x.ReservationSeats)!.ThenInclude(rs => rs.Seat)!.ThenInclude(s => s!.SeatType));
        if (r == null)
            return null;

        return new GetBookingTicketsDTO
        {
            ReservationId = r.Id,
            BookingCode = r.Code,
            Status = r.Status,
            UserId = r.UserId,
            CustomerName = r.User == null ? string.Empty : $"{r.User.FirstName} {r.User.LastName}".Trim(),
            EventId = r.EventId,
            EventTitle = r.Event?.Title ?? string.Empty,
            ImagePath = r.Event?.ImagePath ?? string.Empty,
            EventDate = r.Event?.EventDate ?? default,
            LocationName = r.Event?.GeneralLocation?.Name ?? string.Empty,
            LocationAddress = r.Event?.GeneralLocation == null ? string.Empty : $"{r.Event.GeneralLocation.Address}, {r.Event.GeneralLocation.City}",
            HallName = r.Event?.Hall?.Name ?? string.Empty,
            Tickets = (r.ReservationSeats ?? [])
                .OrderBy(t => t.Seat?.SeatRow).ThenBy(t => t.Seat?.SeatColumn)
                .Select(t => new GetTicketDTO
                {
                    TicketCode = t.TicketCode,
                    SeatName = t.Seat?.SeatName ?? string.Empty,
                    Row = t.Seat?.SeatRow ?? 0,
                    Column = t.Seat?.SeatColumn ?? 0,
                    SeatType = t.Seat?.SeatType?.Name ?? string.Empty,
                    Price = t.Price,
                    CheckedInAt = t.CheckedInAt,
                    CheckedInBy = t.CheckedInBy
                })
                .ToList()
        };
    }

    public async Task<string?> GetTicketOwnerAsync(string ticketCode)
    {
        var code = _normalize(ticketCode);
        return await _reservationSeatRepository.GetAll(t => t.TicketCode == code)
            .Select(t => t.Reservation!.UserId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<CheckInEventDTO>> GetCheckInEventsAsync()
    {
        var now = DateTime.UtcNow;
        var from = now - _checkInLookBack;
        var to = now + _checkInLookAhead;

        var events = await _eventRepository.GetAll(e => e.EventDate >= from && e.EventDate <= to)
            .OrderBy(e => e.EventDate)
            .Select(e => new CheckInEventDTO
            {
                EventId = e.Id,
                Title = e.Title,
                EventDate = e.EventDate,
                LocationName = e.GeneralLocation.Name
            })
            .ToListAsync();

        var ids = events.Select(e => e.EventId).ToList();
        var counts = await _validTickets()
            .Where(t => ids.Contains(t.Reservation!.EventId))
            .GroupBy(t => t.Reservation!.EventId)
            .Select(g => new { EventId = g.Key, Tickets = g.Count(), CheckedIn = g.Count(t => t.CheckedInAt != null) })
            .ToListAsync();

        foreach (var e in events)
        {
            var c = counts.FirstOrDefault(x => x.EventId == e.EventId);
            e.TicketCount = c?.Tickets ?? 0;
            e.CheckedInCount = c?.CheckedIn ?? 0;
        }
        return events;
    }

    public async Task<CheckInResultDTO> CheckInAsync(string ticketCode, int eventId, string staffName)
    {
        var code = _normalize(ticketCode);
        var ticket = code.Length == 0 ? null : await _reservationSeatRepository.GetAsync(t => t.TicketCode == code,
            include: q => q.Include(t => t.Reservation!).ThenInclude(r => r.User)
                .Include(t => t.Reservation!).ThenInclude(r => r.Event)
                .Include(t => t.Seat!).ThenInclude(s => s.SeatType));

        if (ticket?.Reservation == null)
            return await _withCounts(new CheckInResultDTO { Outcome = CheckInOutcome.Unknown, Message = "No ticket with this code." }, eventId);

        var result = _describe(ticket);
        var reservation = ticket.Reservation;

        if (reservation.EventId != eventId)
        {
            result.Outcome = CheckInOutcome.WrongEvent;
            result.Message = $"This ticket is for {reservation.Event?.Title} on {reservation.Event?.EventDate.ToString("d MMM, HH:mm", CultureInfo.InvariantCulture)}.";
            return await _withCounts(result, eventId);
        }

        if (reservation.Status != ReservationStatus.Confirmed)
        {
            result.Outcome = CheckInOutcome.NotValid;
            result.Message = $"Booking {reservation.Code} is {reservation.Status.ToString().ToLowerInvariant()}. Do not admit.";
            return await _withCounts(result, eventId);
        }

        var admitted = ticket.CheckedInAt == null
                       && await _reservationSeatRepository.CheckInAsync(ticket.Id, DateTime.UtcNow, staffName) == 1;
        if (admitted)
        {
            result.Outcome = CheckInOutcome.Admitted;
            result.Message = "Valid ticket. Admit one.";
            return await _withCounts(result, eventId);
        }

        // someone (maybe another door, a moment ago) already used it: show when and who
        var used = await _reservationSeatRepository.GetAll(t => t.Id == ticket.Id)
            .AsNoTracking()
            .Select(t => new { t.CheckedInAt, t.CheckedInBy })
            .FirstAsync();
        result.Outcome = CheckInOutcome.AlreadyUsed;
        result.CheckedInAt = used.CheckedInAt;
        result.CheckedInBy = used.CheckedInBy;
        result.Message = $"Already used at {used.CheckedInAt?.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture)} by {used.CheckedInBy}.";
        return await _withCounts(result, eventId);
    }

    // tickets of confirmed bookings: the ones that are allowed in
    private IQueryable<ReservationSeat> _validTickets() =>
        _reservationSeatRepository.GetAll(t => t.Reservation!.Status == ReservationStatus.Confirmed);

    private async Task<CheckInResultDTO> _withCounts(CheckInResultDTO result, int eventId)
    {
        var tickets = _validTickets().Where(t => t.Reservation!.EventId == eventId);
        result.TicketCount = await tickets.CountAsync();
        result.CheckedInCount = await tickets.CountAsync(t => t.CheckedInAt != null);
        return result;
    }

    private static CheckInResultDTO _describe(ReservationSeat ticket) => new()
    {
        BookingCode = ticket.Reservation?.Code,
        CustomerName = ticket.Reservation?.User == null ? null : $"{ticket.Reservation.User.FirstName} {ticket.Reservation.User.LastName}".Trim(),
        SeatName = ticket.Seat?.SeatName,
        SeatType = ticket.Seat?.SeatType?.Name,
        EventTitle = ticket.Reservation?.Event?.Title,
        CheckedInAt = ticket.CheckedInAt,
        CheckedInBy = ticket.CheckedInBy
    };

    // scanners and people add spaces, dashes and lower case; codes are stored upper case without them
    private static string _normalize(string code) =>
        new string(code.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
}
