using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SliderDTO;

public class GetSliderDTO : IDTO
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = null!;
}
