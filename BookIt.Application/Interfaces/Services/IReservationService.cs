using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services;

public interface IReservationService : IGetService<GetReservationDTO>, IModifyService<CreateReservationDTO, UpdateReservationDTO>
{
    Task<bool> ConfirmReservationAsync(int id, string? note);
    /// <summary>Frees the seats; a paid booking also gets a CancellationRefund (refund owed). Refused once the event has happened.</summary>
    Task<bool> CancelReservationAsync(int id, ModelStateDictionary modelState);
    /// <summary>Marks the owed refund as paid out and the reservation as Refunded.</summary>
    Task<bool> MarkRefundedAsync(int id, ModelStateDictionary modelState);
    Task<bool> AddSeatsToReservationAsync(int reservationId, List<int> seatIds);
    Task<bool> RemoveSeatsFromReservationAsync(int reservationId, List<int> seatIds);
    Task<List<GetReservationDTO>> GetByUserAsync(string userId);

}
