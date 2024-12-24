using BookIt.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookIt.Core.Domain.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        public string LocationAddress { get; set; } = string.Empty;

        [Required]
        public string LocationCity { get; set; } = string.Empty;

        [Required]
        public string LocationCountry { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        public int OrganizerId { get; set; }

        [ForeignKey("OrganizerId")]
        public User? Organizer { get; set; }

        public List<EventPricing> Pricing { get; set; } = new();

        public List<string> Tags { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class EventPricing : ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
