using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SeatDTO;

public class SeatFilterDTO : IDTO
{
    public int SelectedGeneralLocationId { get; set; }
    public int SelectedHallId { get; set; }
    public int SelectedSeatTypeId { get; set; }


    public IEnumerable<SelectListItem> GeneralLocations { get; set; } = [];
    public IEnumerable<SelectListItem> Halls { get; set; } = [];
    public IEnumerable<SelectListItem> SeatTypes { get; set; } = [];
    
    public IEnumerable<GetSeatDTO> Seats { get; set; } = [];


}
