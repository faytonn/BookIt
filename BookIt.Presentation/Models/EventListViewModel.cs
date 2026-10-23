using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Models;

public class EventListViewModel
{
    public EventFilterDTO Filter { get; set; } = new();
    public PaginateDTO<GetEventDTO> Events { get; set; } = new();
    public List<SelectListItem> Locations { get; set; } = [];
    public List<SelectListItem> Categories { get; set; } = [];
}
