namespace BookIt.Application.DTOs.ChatDTO;

public class ConnectionDTO
{
    public int UserId { get; set; }
    public List<string> ConnectionIds { get; set; } = [];
}
