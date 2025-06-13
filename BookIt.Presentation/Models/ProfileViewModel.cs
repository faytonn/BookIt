using BookIt.Application.DTOs.ReservationDTO;

namespace BookIt.Presentation.Models;

public class ProfileViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<GetReservationDTO> Reservations { get; set; } = new();
}