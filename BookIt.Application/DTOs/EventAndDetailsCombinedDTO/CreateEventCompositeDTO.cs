using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Application.DTOs.EventAndDetailsCombinedDTO;

public class CreateEventCompositeDTO : IDTO
{
    public CreateEventDTO Event { get; set; } = new CreateEventDTO();
    public List<CreateEventDetailDTO> EventDetail { get; set; } = new List<CreateEventDetailDTO>();


    //this is for my event section
    public List<SelectListItem> GeneralLocations { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();


    //this is for my eventdetail section
    public List<SelectListItem> Halls { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Languages { get; set; } = new List<SelectListItem>();
}
