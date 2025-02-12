using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Application.DTOs.EventAndDetailsCombinedDTO;

public class UpdateEventCompositeDTO : IDTO
{
    public UpdateEventDTO Event { get; set; } = new UpdateEventDTO();
    public List<UpdateEventDetailDTO> EventDetail { get; set; } = new List<UpdateEventDetailDTO>();

    public List<SelectListItem> GeneralLocations { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Halls { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Languages { get; set; } = new List<SelectListItem>();
}

