using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities
{
    public class Message : BaseAuditableEntity
    {
        public string Text { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}
