using BookIt.Core.Application.DTOs;

namespace BookIt.Core.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponseDTO> CreateReservationAsync(int userId, CreateReservationDTO reservationDto);
        Task<ReservationResponseDTO> GetReservationByIdAsync(int id);
        Task<List<ReservationResponseDTO>> GetUserReservationsAsync(string email);
        Task<bool> DeleteReservationAsync(int id, string email);
    }
}
