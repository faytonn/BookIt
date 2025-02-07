using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs;

public class GetUserDTO : IDTO
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<Role> Roles { get; set; } = [];
    public bool IsActive { get; set; }
    public int ChatId { get; set; }
}
