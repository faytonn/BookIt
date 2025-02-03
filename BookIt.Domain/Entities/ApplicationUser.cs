using Microsoft.AspNetCore.Identity;

namespace BookIt.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set;} = null!;
    public string Address { get; set; } = null!;
    public bool IsActive { get; set; } = true;

}
