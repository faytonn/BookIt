using BookIt.Core.Application.DTOs;

namespace BookIt.Core.Application.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDTO> CreateEventAsync(int organizerId, CreateEventDTO eventDto);
        Task<List<EventResponseDTO>> GetEventsAsync(string? type = null, string? search = null);
        Task<EventResponseDTO?> GetEventByIdAsync(int id);
        Task<EventResponseDTO> UpdateEventAsync(int id, int organizerId, CreateEventDTO eventDto);
        Task<bool> DeleteEventAsync(int id, int organizerId);
    }
}
