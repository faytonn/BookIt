using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.GeneralLocationDTO;

public class CreateGeneralLocationDTO : IDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}
