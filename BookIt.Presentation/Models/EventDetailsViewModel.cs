using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;

namespace BookIt.Presentation.Models;

public class EventDetailsViewModel
{
    public GetEventDTO Event { get; set; } = new();
    public GetEventDetailDTO? Detail { get; set; }
}
