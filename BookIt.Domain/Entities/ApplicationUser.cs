using Microsoft.AspNetCore.Identity;

namespace BookIt.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set;} = null!;
    public string Address { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public bool IsSubscribed { get; set; } = false;

    public ICollection<Reservation>? Reservations { get; set; }
    public ICollection<Chat>? Chats { get; set; }
    public ICollection<Message>? Messages { get; set; }

}
