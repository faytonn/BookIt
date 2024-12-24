using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.Application.DTOs
{
    public class CreateEventDTO
    {
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
        public LocationDTO Location { get; set; } = new();

        public IFormFile? Image { get; set; }

        [Required]
        public List<PricingDTO> Pricing { get; set; } = new();

        public List<string> Tags { get; set; } = new();
    }

    public class LocationDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
    }

    public class PricingDTO
    {
        public int Id { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }
    }

    public class OrganizerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDTO Location { get; set; } = new();
        public string? ImageUrl { get; set; }
        public List<PricingDTO> Pricing { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public OrganizerDTO Organizer { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
