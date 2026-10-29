using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.ReservationDTO;

public class CreateReservationDTO : IDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select an event.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Enter the customer's email.")]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    // resolved from CustomerEmail by the controller, never posted by the form
    public string UserId { get; set; } = string.Empty;

    [MinLength(1, ErrorMessage = "Please select at least one seat.")]
    public List<int> SeatIds { get; set; } = [];
}
