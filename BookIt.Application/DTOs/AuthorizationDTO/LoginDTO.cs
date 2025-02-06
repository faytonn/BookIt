namespace BookIt.Application.DTOs.AuthorizationDTO;

public class LoginDTO
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool RememberMe { get; set; }
}
public class LoginReturnDTO
{
    public bool ModelState { get; set; } = false;
    public string? ReturnUrl { get; set; }
}