//using AutoMapper;
//using BookIt.Application.DTOs.Common;
//using BookIt.Application.DTOs.ReservationDTO;
//using BookIt.Application.Interfaces.Repositories;
//using BookIt.Application.Interfaces.Services;
//using BookIt.Domain.Entities;
//using BookIt.Domain.Enums;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace BookIt.Persistence.Implementations.Services;

//public class ReservationService : IReservationService
//{
//    private readonly IReservationRepository _reservationRepository;
//    private readonly ISeatRepository _seatRepository;
//    private readonly IReservationSeatRepository _reservationSeatRepository;
//    private readonly IEventRepository _eventRepository;
//    private readonly IMapper _mapper;

//    public ReservationService(IReservationRepository reservationRepository, ISeatRepository seatRepository, IReservationSeatRepository reservationSeatRepository, IEventRepository eventRepository, IMapper mapper)
//    {
//        _reservationRepository = reservationRepository;
//        _seatRepository = seatRepository;
//        _reservationSeatRepository = reservationSeatRepository;
//        _eventRepository = eventRepository;
//        _mapper = mapper;
//    }

//    public async Task<GetReservationDTO> GetAsync(int id, LanguageType language = LanguageType.English)
//    {
//        var reservation = await _reservationRepository.GetAsync(id);
//        if (reservation == null)
//            return null;

//        var dto = _mapper.Map<GetReservationDTO>(reservation);

//        dto.Seats = await MapReservationSeats(reservation.Id);

//        return dto;
//    }

//    public List<GetReservationDTO> GetAll(LanguageType language = LanguageType.English)
//    {
//        var reservations = _reservationRepository.GetAll();
//        var dtos = new List<GetReservationDTO>();

//        foreach (var r in reservations)
//        {
//            var dto = _mapper.Map<GetReservationDTO>(r);
//            dto.Seats = MapReservationSeats(r.Id).Result;
//            dtos.Add(dto);
//        }

//        return dtos;
//    }

//    public async Task<PaginateDTO<GetReservationDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
//    {
//        var totalCount = await _reservationRepository.CountAsync();

//        var reservations = await _reservationRepository.GetPageAsync(page, limit);

//        var dtos = _mapper.Map<List<GetReservationDTO>>(reservations);

//        var result = new PaginateDTO<GetReservationDTO>
//        {
//            Items = dtos,
//            CurrentPage = page,
//            PageSize = limit,
//            TotalItems = totalCount
//        };


//        return result;
//    }

//    public async Task<bool> IsExistAsync(int id)
//    {
//        return await _reservationRepository.IsExistAsync(r => r.Id == id);
//    }


//    #region ========== IModifyService<CreateReservationDTO, UpdateReservationDTO> ==========

//    public async Task<bool> CreateAsync(CreateReservationDTO dto, ModelStateDictionary modelState)
//    {
//        // 1) Basic validation
//        if (!modelState.IsValid)
//            return false;

//        var ev = await _eventRepository.GetAsync(dto.EventId);
//        if (ev == null)
//        {
//            modelState.AddModelError("EventId", "Event not found.");
//            return false;
//        }

//        // 2) Check seat availability (for each seat in dto.SeatIds)
//        foreach (var seatId in dto.SeatIds)
//        {
//            bool isAvailable = await IsSeatFreeForEventAsync(seatId, ev.Id);
//            if (!isAvailable)
//            {
//                modelState.AddModelError("SeatIds", $"SeatId {seatId} is not available.");
//                return false;
//            }
//        }

//        var reservation = new Reservation
//        {
//            EventId = ev.Id,
//            UserId = dto.UserId,
//            ReservationDate = DateTime.UtcNow,
//            Status = ReservationStatus.Pending,
//            NumberOfTickets = dto.SeatIds.Count,
//        };

//        await _reservationRepository.CreateAsync(reservation);

//        // 4) For each seat, create a bridging entity
//        decimal totalAmount = 0;
//        var reservationSeats = new List<ReservationSeat>();

//        foreach (var seatId in dto.SeatIds)
//        {
//            var seat = await _seatRepository.GetAsync(seatId);
//            if (seat == null)
//            {
//                modelState.AddModelError("SeatIds", $"SeatId {seatId} not found.");
//                return false;
//            }

//            decimal seatPrice = await GetSeatPriceForEventAsync(seat, ev);
//            totalAmount += seatPrice;

//            var rs = new ReservationSeat
//            {
//                ReservationId = reservation.Id,
//                SeatId = seat.Id,
//                Price = seatPrice
//            };
//            reservationSeats.Add(rs);
//        }

//        // 5) Update total on the reservation
//        reservation.TotalAmount = totalAmount;
//        _reservationRepository.Update(reservation);

//        // 6) Insert bridging records
//        await _reservationSeatRepository.CreateRangeAsync(reservationSeats);

//        return true;
//    }

//    public async Task<bool> UpdateAsync(UpdateReservationDTO dto, ModelStateDictionary modelState)
//    {
//        if (!modelState.IsValid)
//            return false;

//        var reservation = await _reservationRepository.GetAsync(dto.Id);
//        if (reservation == null)
//        {
//            modelState.AddModelError("Id", "Reservation not found.");
//            return false;
//        }

//        reservation.EventId = dto.EventId;
//        reservation.Status = dto.ReservationStatus;

//        _reservationRepository.Update(reservation);
//        return true;
//    }

//    public async Task<UpdateReservationDTO> GetUpdatedDtoAsync(int id)
//    {
//        var reservation = await _reservationRepository.GetAsync(id);
//        if (reservation == null) return null;

//        var dto = new UpdateReservationDTO
//        {
//            Id = reservation.Id,
//            EventId = reservation.EventId,
//            ReservationStatus = reservation.Status,
//            // If you want seat IDs, gather them from ReservationSeat
//            SeatIds = await _reservationSeatRepository
//                          .GetSeatIdsByReservationAsync(reservation.Id)
//        };
//        return dto;
//    }

//    public async Task DeleteAsync(int id)
//    {
//        var reservation = await _reservationRepository.GetAsync(id);
//        if (reservation == null) return;

//        // Usually you'd set the reservation to Cancelled instead of a hard delete,
//        // but if you truly want to remove it from the DB:
//        _reservationRepository.HardDelete(reservation);

//        // Also remove bridging records
//        var seats = await _reservationSeatRepository.GetByReservationIdAsync(id);
//        foreach (var seat in seats)
//        {
//            _reservationSeatRepository.HardDelete(seat);
//        }
//    }

//    #endregion

//    #region ========== Additional Specialized Methods ==========

//    public async Task<bool> ConfirmReservationAsync(int reservationId, string paymentTxId)
//    {
//        var reservation = await _reservationRepository.GetAsync(reservationId);
//        if (reservation == null) return false;
//        if (reservation.Status != ReservationStatus.Pending) return false;

//        reservation.Status = ReservationStatus.Confirmed;
//        _reservationRepository.Update(reservation);
//        return true;
//    }

//    public async Task<bool> CancelReservationAsync(int reservationId)
//    {
//        var reservation = await _reservationRepository.GetAsync(reservationId);
//        if (reservation == null) return false;

//        reservation.Status = ReservationStatus.Cancelled;
//        _reservationRepository.Update(reservation);


//        return true;
//    }

//    public async Task<bool> AddSeatsToReservationAsync(int reservationId, List<int> seatIds)
//    {
//        var reservation = await _reservationRepository.GetAsync(reservationId);
//        if (reservation == null) return false;

//        // Check if current status allows adding seats
//        // e.g., if (reservation.Status != ReservationStatus.PendingPayment) { return false; }

//        var ev = await _eventRepository.GetAsync(reservation.EventId);
//        if (ev == null) return false;

//        foreach (var seatId in seatIds)
//        {
//            bool isAvailable = await IsSeatFreeForEventAsync(seatId, ev.Id);
//            if (!isAvailable) return false;
//        }

//        decimal additionalCost = 0;
//        var newBridgingRecords = new List<ReservationSeat>();
//        foreach (var seatId in seatIds)
//        {
//            var seat = await _seatRepository.GetAsync(seatId);
//            if (seat == null) return false;

//            decimal seatPrice = await GetSeatPriceForEventAsync(seat, ev);
//            additionalCost += seatPrice;

//            newBridgingRecords.Add(new ReservationSeat
//            {
//                ReservationId = reservationId,
//                SeatId = seatId,
//                Price = seatPrice
//            });
//        }

//        reservation.TotalAmount += additionalCost;
//        reservation.NumberOfTickets += seatIds.Count;
//        _reservationRepository.Update(reservation);

//        await _reservationSeatRepository.CreateRangeAsync(newBridgingRecords);

//        return true;
//    }

//    public async Task<bool> RemoveSeatsFromReservationAsync(int reservationId, List<int> seatIds)
//    {
//        var reservation = await _reservationRepository.GetAsync(reservationId);
//        if (reservation == null) return false;

//        // fetch the bridging records
//        var bridgingRecords = await _reservationSeatRepository.GetByReservationIdAsync(reservationId);
//        var toRemove = bridgingRecords.Where(rs => seatIds.Contains(rs.SeatId)).ToList();

//        if (!toRemove.Any())
//            return false;

//        decimal refund = toRemove.Sum(x => x.Price);

//        // remove bridging entries
//        foreach (var rec in toRemove)
//        {
//            _reservationSeatRepository.HardDelete(rec);
//        }

//        // update reservation
//        reservation.TotalAmount -= refund;
//        reservation.NumberOfTickets -= toRemove.Count;
//        _reservationRepository.Update(reservation);

//        return true;
//    }

//    #endregion

//    #region ========== Private Helpers ==========

//    private async Task<List<GetReservationSeatDTO>> MapReservationSeats(int reservationId)
//    {
//        var reservationSeats = await _reservationSeatRepository.GetByReservationIdAsync(reservationId);
//        var seatDtos = new List<GetReservationSeatDTO>();

//        foreach (var rSeat in reservationSeats)
//        {
//            var seat = rSeat.Seat;
//            if (seat == null) continue;

//            seatDtos.Add(new GetReservationSeatDTO
//            {
//                SeatId = seat.Id,
//                SeatName = seat.SeatName,
//                Row = seat.SeatRow,
//                Column = seat.SeatColumn,
//                Price = rSeat.Price
//            });
//        }
//        return seatDtos;
//    }

//    private async Task<bool> IsSeatFreeForEventAsync(int seatId, int eventId)
//    {
//        // This is domain-specific. You can check:
//        // 1) If the seat belongs to the Hall used by the event (if needed).
//        // 2) Whether there's already an ACTIVE (not cancelled) reservation bridging record for this seat + event.

//        // For demonstration:
//        var activeBridges = await _reservationSeatRepository.GetActiveBySeatAndEventAsync(seatId, eventId);
//        return !activeBridges.Any();
//    }

//    private async Task<decimal> GetSeatPriceForEventAsync(Seat seat, Event ev)
//    {
//        return seat.SeatType?.DefaultPrice ?? 0m;
//    }

//    public async Task<bool> AddSeatsToReservationsAsync(int reservationId, List<int> seatIds)
//    {
//        var reservation = await _reservationRepository.GetAsync(reservationId);
//        if (reservation == null) return false;


//        var ev = await _eventRepository.GetAsync(reservation.EventId);
//        if (ev == null) return false;

//        foreach (var seatId in seatIds)
//        {
//            bool isAvailable = await IsSeatFreeForEventAsync(seatId, ev.Id);
//            if (!isAvailable) return false;
//        }

//        decimal additionalCost = 0;
//        var newBridgingRecords = new List<ReservationSeat>();
//        foreach (var seatId in seatIds)
//        {
//            var seat = await _seatRepository.GetAsync(seatId);
//            if (seat == null) return false;

//            decimal seatPrice = await GetSeatPriceForEventAsync(seat, ev);
//            additionalCost += seatPrice;

//            newBridgingRecords.Add(new ReservationSeat
//            {
//                ReservationId = reservationId,
//                SeatId = seatId,
//                Price = seatPrice
//            });
//        }

//        reservation.TotalAmount += additionalCost;
//        reservation.NumberOfTickets += seatIds.Count;
//        _reservationRepository.HardDelete(reservation);

//        await _reservationSeatRepository.CreateRangeAsync(newBridgingRecords);

//        return true;

//    }

//    #endregion
//}
