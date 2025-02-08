using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Interfaces.Services.Generic;

namespace BookIt.Application.Interfaces.Services;

public interface IReservationService : IGetService<GetReservationDTO>, IModifyService<CreateReservationDTO, UpdateReservationDTO>
{
    //Task<int> CreateReservationAsync(int eventId, string userId, List<int> seatIds);
    Task<bool> ConfirmReservationAsync(int reservationId, string paymentTxId);
    Task<bool> CancelReservationAsync(int reservationId);
    //Task<bool> CancelReservationAsync(int reservationId, string paymentTxId);
    Task<bool> AddSeatsToReservationsAsync(int reservationId, List<int> seatIds);
    Task<bool> RemoveSeatsFromReservationAsync(int reservationId, List<int> seatIds);

}
