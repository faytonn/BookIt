using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Application.DTOs.EventAndDetailsCombinedDTO;

public class EventCombinedDTO : IDTO
{
    public CreateEventDTO Event { get; set; } = new CreateEventDTO();
    public CreateEventDetailDTO EventDetail { get; set; } = new CreateEventDetailDTO();


    //this is for my event section
    public IEnumerable<SelectListItem> GeneralLocations { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

   
    //this is for my eventdetail section
    public IEnumerable<SelectListItem> Halls { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Languages { get; set; } = new List<SelectListItem>();
}
