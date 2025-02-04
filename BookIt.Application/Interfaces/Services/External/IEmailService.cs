using BookIt.Application.DTOs;

namespace BookIt.Application.Interfaces.Services.External;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailDTO dto);
}
