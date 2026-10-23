using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.ReservationDTO;

public class CreateReservationDTO : IDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select an event.")]
    public int EventId { get; set; }
    public string UserId { get; set; } = null!;
    [MinLength(1, ErrorMessage = "Please select at least one seat.")]
    public List<int> SeatIds { get; set; } = [];
}
