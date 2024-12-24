namespace BookIt.Core.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public string? ProfileImage { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool MarketingEmails { get; set; }
        public bool TwoFactorAuth { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpiry { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation property for events organized by the user
        public virtual ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    }
}
