using BookIt.Domain.Entities.Common;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookIt.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsSubscribed { get; set; } = false;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public LanguageType SelectedLanguage { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
    public virtual ICollection<Message> Messages { get; set; }

    public ApplicationUser()
    {
        Reservations = new HashSet<Reservation>();
        Chats = new HashSet<Chat>();
        Messages = new HashSet<Message>();
    }
}
