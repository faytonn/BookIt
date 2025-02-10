using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.GeneralLocationDTO;

public class UpdateGeneralLocationDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}