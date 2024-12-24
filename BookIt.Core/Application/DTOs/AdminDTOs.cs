using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.Application.DTOs
{
    public class UpdateUserStatusDTO
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateUserRoleDTO
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
