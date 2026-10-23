using AutoMapper;
using BookIt.Application.DTOs.EventSeatDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class EventSeatService : IEventSeatService
{
    private readonly IEventSeatRepository _eventSeatRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public EventSeatService(
        IEventSeatRepository eventSeatRepository,
        IEventRepository eventRepository,
        ISeatRepository seatRepository,
        IReservationRepository reservationRepository,
        IMapper mapper)
    {
        _eventSeatRepository = eventSeatRepository;
        _eventRepository = eventRepository;
        _seatRepository = seatRepository;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<List<GetEventSeatDTO>> GetSeatMapAsync(int eventId, string? forUserId = null)
    {
        if (!await _eventRepository.IsExistAsync(e => e.Id == eventId))
            throw new NotFoundException("Event not found.");

        var now = DateTime.UtcNow;
        var seats = await _eventSeatRepository.GetByEventAsync(eventId);
        var dtos = _mapper.Map<List<GetEventSeatDTO>>(seats);

        for (var i = 0; i < seats.Count; i++)
        {
            var es = seats[i];
            var dto = dtos[i];
            if (es.Status == EventSeatStatus.Held && !_isActiveHold(es, now))
            {
                dto.Status = EventSeatStatus.Available;
                dto.HeldUntil = null;
            }
            dto.HeldByMe = dto.Status == EventSeatStatus.Held && forUserId != null && es.HeldByUserId == forUserId;
        }

        return dtos;
    }

    public async Task<HoldResultDTO> HoldAsync(int eventId, int seatId, string userId)
    {
        var es = await _eventSeatRepository.GetByEventAndSeatAsync(eventId, seatId);
        if (es == null)
            return HoldResultDTO.Of(HoldOutcome.NotFound);

        var now = DateTime.UtcNow;

        if (es.Status == EventSeatStatus.Sold)
            return HoldResultDTO.Of(HoldOutcome.Conflict);

        if (es.Status == EventSeatStatus.Held && _isActiveHold(es, now) && es.HeldByUserId != userId)
            return HoldResultDTO.Of(HoldOutcome.Conflict);

        // Available, expired hold, or the same user refreshing their own hold.
        es.Status = EventSeatStatus.Held;
        es.HeldUntil = now.Add(EventSeat.HoldDuration);
        es.HeldByUserId = userId;
        _eventSeatRepository.Update(es);

        try
        {
            await _eventSeatRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Another request changed this row between our read and our write: it won.
            return HoldResultDTO.Of(HoldOutcome.Conflict);
        }

        return new HoldResultDTO
        {
            Outcome = HoldOutcome.Success,
            EventSeatId = es.Id,
            HeldUntil = es.HeldUntil
        };
    }

    public async Task<HoldResultDTO> ConfirmAsync(int eventSeatId, string userId)
    {
        var es = await _eventSeatRepository.GetAsync(x => x.Id == eventSeatId,
            include: q => q.Include(x => x.Seat).ThenInclude(s => s.SeatType));
        if (es == null)
            return HoldResultDTO.Of(HoldOutcome.NotFound);

        var now = DateTime.UtcNow;

        if (es.Status == EventSeatStatus.Sold)
            return HoldResultDTO.Of(HoldOutcome.Conflict);

        if (es.Status != EventSeatStatus.Held || es.HeldByUserId != userId || !_isActiveHold(es, now))
            return HoldResultDTO.Of(HoldOutcome.HoldExpired);

        var price = es.Seat.SeatType.DefaultPrice;
        var reservation = new Reservation
        {
            EventId = es.EventId,
            UserId = userId,
            ReservationDate = now,
            Status = ReservationStatus.Confirmed,
            NumberOfTickets = 1,
            TotalAmount = price,
            ReservationSeats = new List<ReservationSeat>
            {
                new() { SeatId = es.SeatId, Price = price }
            }
        };
        await _reservationRepository.CreateAsync(reservation);

        es.Status = EventSeatStatus.Sold;
        es.HeldUntil = null;
        es.Reservation = reservation;
        _eventSeatRepository.Update(es);

        try
        {
            // one SaveChanges: the reservation, its seat line and the EventSeat flip together
            await _eventSeatRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return HoldResultDTO.Of(HoldOutcome.Conflict);
        }

        return new HoldResultDTO
        {
            Outcome = HoldOutcome.Success,
            EventSeatId = es.Id,
            ReservationId = reservation.Id
        };
    }

    public async Task<HoldResultDTO> ReleaseAsync(int eventSeatId, string userId)
    {
        var es = await _eventSeatRepository.GetAsync(eventSeatId);
        if (es == null)
            return HoldResultDTO.Of(HoldOutcome.NotFound);

        if (es.Status != EventSeatStatus.Held || es.HeldByUserId != userId)
            return HoldResultDTO.Of(HoldOutcome.HoldExpired);

        es.Status = EventSeatStatus.Available;
        es.HeldUntil = null;
        es.HeldByUserId = null;
        _eventSeatRepository.Update(es);

        try
        {
            await _eventSeatRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return HoldResultDTO.Of(HoldOutcome.Conflict);
        }

        return new HoldResultDTO { Outcome = HoldOutcome.Success, EventSeatId = es.Id };
    }

    public async Task<int> ReleaseExpiredHoldsAsync()
    {
        return await _eventSeatRepository.ReleaseExpiredHoldsAsync(DateTime.UtcNow);
    }

    public async Task<int> EnsureSeatsForEventAsync(int eventId)
    {
        var ev = await _eventRepository.GetAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event not found.");

        var existing = await _eventSeatRepository.GetAll(es => es.EventId == eventId)
            .Select(es => es.SeatId)
            .ToListAsync();
        var existingSet = existing.ToHashSet();

        var missing = await _seatRepository.GetAll(s => s.HallId == ev.HallId)
            .Select(s => s.Id)
            .ToListAsync();

        var rows = missing
            .Where(seatId => !existingSet.Contains(seatId))
            .Select(seatId => new EventSeat { EventId = eventId, SeatId = seatId, Status = EventSeatStatus.Available })
            .ToList();

        if (rows.Count > 0)
            await _eventSeatRepository.CreateRangeAsync(rows);

        return rows.Count;
    }

    private static bool _isActiveHold(EventSeat es, DateTime now)
    {
        return es.Status == EventSeatStatus.Held && es.HeldUntil.HasValue && es.HeldUntil.Value > now;
    }
}
