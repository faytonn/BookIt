using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.SliderDTO;

public class CreateSliderDTO : IDTO
{
    public string ImagePath { get; set; } = null!;
}
