namespace BookIt.Application.DTOs.EventSeatTypeDTO;

public class CreateEventSeatTypeDTO
{
    public int EventId { get; set; }
    public int SeatTypeId { get; set; }
    public decimal Price { get; set; }
    public string? AdditionalDetails { get; set; }
}
