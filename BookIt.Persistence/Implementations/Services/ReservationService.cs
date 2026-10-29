using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IReservationSeatRepository _reservationSeatRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventSeatRepository _eventSeatRepository;
    private readonly ICancellationRefundRepository _cancellationRefundRepository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository, ISeatRepository seatRepository, IReservationSeatRepository reservationSeatRepository, IEventRepository eventRepository, IEventSeatRepository eventSeatRepository, ICancellationRefundRepository cancellationRefundRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _seatRepository = seatRepository;
        _reservationSeatRepository = reservationSeatRepository;
        _eventRepository = eventRepository;
        _eventSeatRepository = eventSeatRepository;
        _cancellationRefundRepository = cancellationRefundRepository;
        _mapper = mapper;
    }

    public async Task<GetReservationDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var reservation = await _reservationRepository.GetWithDetailsAsync(id);
        if (reservation == null)
            throw new NotFoundException("Reservation not found.");

        return _mapper.Map<GetReservationDTO>(reservation);
    }

    public List<GetReservationDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var reservations = _reservationRepository.GetAllWithDetails();
        return _mapper.Map<List<GetReservationDTO>>(reservations);
    }

    public async Task<PaginateDTO<GetReservationDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var totalCount = await _reservationRepository.CountAsync();
        var pageCount = (int)Math.Ceiling((decimal)totalCount / limit);
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;

        var reservations = await _reservationRepository.GetPageAsync(page, limit);

        return new PaginateDTO<GetReservationDTO>
        {
            Items = _mapper.Map<List<GetReservationDTO>>(reservations),
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = limit,
            PageCount = pageCount
        };
    }

    public async Task<List<GetReservationDTO>> GetByUserAsync(string userId)
    {
        var reservations = await _reservationRepository.GetByUserAsync(userId);
        return _mapper.Map<List<GetReservationDTO>>(reservations);
    }


    public async Task<bool> IsExistAsync(int id)
    {
        return await _reservationRepository.IsExistAsync(r => r.Id == id);
    }

    public async Task<bool> CreateAsync(CreateReservationDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var ev = await _eventRepository.GetAsync(dto.EventId);
        if (ev == null)
        {
            modelState.AddModelError("EventId", "Event not found.");
            return false;
        }

        var now = DateTime.UtcNow;
        if (ev.EventDate < now)
        {
            modelState.AddModelError("EventId", "This event has already taken place.");
            return false;
        }

        // An admin booking is a box-office sale: the seats are sold right away, so the booking is Confirmed.
        // (Pending used to block the seats as Sold with no way to ever expire.)
        var reservation = new Reservation
        {
            EventId = ev.Id,
            UserId = dto.UserId,
            ReservationDate = now,
            Status = ReservationStatus.Confirmed,
            NumberOfTickets = dto.SeatIds.Count,
            ReservationSeats = new List<ReservationSeat>()
        };

        decimal totalAmount = 0;
        foreach (var seatId in dto.SeatIds.Distinct())
        {
            // the EventSeat row (with its rowversion) is what we claim, not the Seat
            var eventSeat = await _eventSeatRepository.GetByEventAndSeatAsync(ev.Id, seatId);
            if (eventSeat == null)
            {
                modelState.AddModelError("SeatIds", $"SeatId {seatId} does not belong to this event.");
                return false;
            }
            if (!_isTakeable(eventSeat, dto.UserId, now))
            {
                modelState.AddModelError("SeatIds", $"SeatId {seatId} is not available.");
                return false;
            }

            decimal seatPrice = eventSeat.Seat.SeatType.DefaultPrice;
            totalAmount += seatPrice;
            reservation.ReservationSeats.Add(new ReservationSeat
            {
                SeatId = seatId,
                Price = seatPrice
            });

            eventSeat.Status = EventSeatStatus.Sold;
            eventSeat.HeldUntil = null;
            eventSeat.HeldByUserId = null;
            eventSeat.Reservation = reservation;
            _eventSeatRepository.Update(eventSeat);
        }
        reservation.TotalAmount = totalAmount;

        await _reservationRepository.CreateAsync(reservation);

        try
        {
            await _reservationRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            modelState.AddModelError("SeatIds", "One of the seats was taken by someone else a moment ago. Please pick again.");
            return false;
        }
        await _eventRepository.RefreshSeatStatsAsync(ev.Id);
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateReservationDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var reservation = await _reservationRepository.GetAsync(dto.Id);
        if (reservation == null)
        {
            modelState.AddModelError("Id", "Reservation not found.");
            return false;
        }

        // Only the status can change here, and only through the real transitions.
        // The event is fixed: the seats belong to that event's hall. Seats change via Add/RemoveSeats.
        if (dto.Status == reservation.Status)
            return true;

        return dto.Status switch
        {
            ReservationStatus.Confirmed => await ConfirmReservationAsync(dto.Id, null) || _fail(modelState, "Only a pending reservation can be confirmed."),
            ReservationStatus.Cancelled => await CancelReservationAsync(dto.Id, modelState),
            ReservationStatus.Refunded => await MarkRefundedAsync(dto.Id, modelState),
            _ => _fail(modelState, $"A reservation can't be moved to {dto.Status} by hand.")
        };
    }

    private static bool _fail(ModelStateDictionary modelState, string message)
    {
        modelState.AddModelError("", message);
        return false;
    }

    public async Task<UpdateReservationDTO> GetUpdatedDtoAsync(int id)
    {
        var reservation = await _reservationRepository.GetAsync(id);
        if (reservation == null)
            throw new NotFoundException("Reservation not found.");
        var dto = new UpdateReservationDTO
        {
            Id = reservation.Id,
            EventId = reservation.EventId,
            Status = reservation.Status,
            SeatIds = await _reservationSeatRepository.GetSeatIdsByReservationAsync(reservation.Id)
        };
        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        //my softdelete is cancel here, because hard deleting does not sound ethical in this case
        await CancelReservationAsync(id, new ModelStateDictionary());
    }

    public async Task<bool> ConfirmReservationAsync(int id, string? note)
    {
        var reservation = await _reservationRepository.GetAsync(id);
        if (reservation == null) return false;
        if (reservation.Status != ReservationStatus.Pending) return false;
        reservation.Status = ReservationStatus.Confirmed;
        _reservationRepository.Update(reservation);
        await _reservationRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelReservationAsync(int id, ModelStateDictionary modelState)
    {
        var reservation = await _reservationRepository.GetAsync(r => r.Id == id,
            include: q => q.Include(r => r.Event).Include(r => r.CancellationRefund));
        if (reservation == null)
            return _fail(modelState, "Reservation not found.");
        if (reservation.Status is ReservationStatus.Cancelled or ReservationStatus.Refunded)
            return _fail(modelState, "This reservation is already cancelled.");

        var now = DateTime.UtcNow;
        if (reservation.Event != null && reservation.Event.EventDate < now)
            return _fail(modelState, "The event has already taken place, so this booking can't be cancelled.");

        // money was taken for a confirmed booking: record what is owed back (TotalAmount stays as the sale price)
        if (reservation.Status == ReservationStatus.Confirmed && reservation.TotalAmount > 0 && reservation.CancellationRefund == null)
        {
            await _cancellationRefundRepository.CreateAsync(new CancellationRefund
            {
                ReservationId = reservation.Id,
                CancellationDate = now,
                RefundAmount = reservation.TotalAmount,
                IsRefunded = false
            });
        }

        reservation.Status = ReservationStatus.Cancelled;
        _reservationRepository.Update(reservation);
        await _releaseSeatsAsync(reservation.Id);
        await _reservationRepository.SaveChangesAsync();
        await _eventRepository.RefreshSeatStatsAsync(reservation.EventId);
        return true;
    }

    public async Task<bool> MarkRefundedAsync(int id, ModelStateDictionary modelState)
    {
        var reservation = await _reservationRepository.GetAsync(r => r.Id == id, include: q => q.Include(r => r.CancellationRefund));
        if (reservation == null)
            return _fail(modelState, "Reservation not found.");
        if (reservation.Status != ReservationStatus.Cancelled || reservation.CancellationRefund == null)
            return _fail(modelState, "Only a cancelled booking with a refund owed can be marked as refunded.");

        reservation.CancellationRefund.IsRefunded = true;
        reservation.Status = ReservationStatus.Refunded;
        _reservationRepository.Update(reservation);
        await _reservationRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSeatsToReservationAsync(int reservationId, List<int> seatIds)   
    {
        var reservation = await _reservationRepository.GetAsync(reservationId);
        if (reservation == null || reservation.Status is ReservationStatus.Cancelled or ReservationStatus.Refunded)
            return false;

        var ev = await _eventRepository.GetAsync(reservation.EventId);

        if (ev == null)
            return false;

        var now = DateTime.UtcNow;
        decimal additionalCost = 0;
        var newBridgingRecords = new List<ReservationSeat>();

        foreach (var seatId in seatIds.Distinct())
        {
            var eventSeat = await _eventSeatRepository.GetByEventAndSeatAsync(ev.Id, seatId);
            if (eventSeat == null || !_isTakeable(eventSeat, reservation.UserId, now))
                return false;

            decimal seatPrice = eventSeat.Seat.SeatType.DefaultPrice;
            additionalCost += seatPrice;
            newBridgingRecords.Add(new ReservationSeat
            {
                ReservationId = reservationId,
                SeatId = seatId,
                Price = seatPrice
            });

            eventSeat.Status = EventSeatStatus.Sold;
            eventSeat.HeldUntil = null;
            eventSeat.HeldByUserId = null;
            eventSeat.ReservationId = reservationId;
            _eventSeatRepository.Update(eventSeat);
        }

        reservation.TotalAmount += additionalCost;
        reservation.NumberOfTickets += newBridgingRecords.Count;
        _reservationRepository.Update(reservation);

        try
        {
            await _reservationSeatRepository.CreateRangeAsync(newBridgingRecords);
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
        await _eventRepository.RefreshSeatStatsAsync(ev.Id);
        return true;
    }

    public async Task<bool> RemoveSeatsFromReservationAsync(int reservationId, List<int> seatIds)
    {
        var reservation = await _reservationRepository.GetAsync(reservationId);
        if (reservation == null) return false;
        var bridgingRecords = await _reservationSeatRepository.GetByReservationIdAsync(reservationId);
        var toRemove = bridgingRecords.Where(rs => seatIds.Contains(rs.SeatId)).ToList();
        if (!toRemove.Any())
            return false;
        decimal refund = toRemove.Sum(x => x.Price);
        foreach (var rec in toRemove)
        {
            _reservationSeatRepository.HardDelete(rec);
            var eventSeat = await _eventSeatRepository.GetByEventAndSeatAsync(reservation.EventId, rec.SeatId);
            if (eventSeat != null && eventSeat.ReservationId == reservation.Id)
            {
                _free(eventSeat);
                _eventSeatRepository.Update(eventSeat);
            }
        }
        reservation.TotalAmount -= refund;
        reservation.NumberOfTickets -= toRemove.Count;
        // a booking with no seats left is not a booking any more
        if (reservation.NumberOfTickets <= 0)
            reservation.Status = ReservationStatus.Cancelled;
        _reservationRepository.Update(reservation);
        await _reservationRepository.SaveChangesAsync();
        await _eventRepository.RefreshSeatStatsAsync(reservation.EventId);
        return true;
    }

    /// <summary>Available, expired hold, or held by the same user.</summary>
    private static bool _isTakeable(EventSeat eventSeat, string userId, DateTime now)
    {
        if (eventSeat.Status == EventSeatStatus.Sold)
            return false;
        if (eventSeat.Status == EventSeatStatus.Available)
            return true;
        return eventSeat.HeldByUserId == userId || !eventSeat.HeldUntil.HasValue || eventSeat.HeldUntil.Value <= now;
    }

    private static void _free(EventSeat eventSeat)
    {
        eventSeat.Status = EventSeatStatus.Available;
        eventSeat.HeldUntil = null;
        eventSeat.HeldByUserId = null;
        eventSeat.ReservationId = null;
    }

    private async Task _releaseSeatsAsync(int reservationId)
    {
        var seats = await _eventSeatRepository.GetAll(es => es.ReservationId == reservationId).ToListAsync();
        foreach (var eventSeat in seats)
        {
            _free(eventSeat);
            _eventSeatRepository.Update(eventSeat);
        }
    }
}