﻿namespace BookIt.Application.DTOs.AuthorizationDTO;

public class ResetPasswordDTO
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Token { get; set; }
}
