using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;

namespace BookIt.Application.DTOs.EventAndDetailsCombinedDTO;

public class GetEventCompositeDTO : IDTO
{
    public GetEventDTO Event { get; set; } = new GetEventDTO();
    public List<GetEventDetailDTO> EventDetails { get; set; } = [];
}
