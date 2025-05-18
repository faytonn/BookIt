using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services;

public interface IReservationService : IGetService<GetReservationDTO>, IModifyService<CreateReservationDTO, UpdateReservationDTO>
{
    Task<bool> ConfirmReservationAsync(int id, string? note);
    Task<bool> CancelReservationAsync(int id);
    Task<bool> AddSeatsToReservationAsync(int reservationId, List<int> seatIds);
    Task<bool> RemoveSeatsFromReservationAsync(int reservationId, List<int> seatIds);
}
