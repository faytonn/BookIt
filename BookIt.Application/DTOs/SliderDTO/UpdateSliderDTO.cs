using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SliderDTO;

public class UpdateSliderDTO : IDTO
{
    public string ImagePath { get; set; } = null!;
}
