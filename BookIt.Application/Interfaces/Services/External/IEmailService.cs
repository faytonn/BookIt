using BookIt.Application.DTOs.EmailDTO;

namespace BookIt.Application.Interfaces.Services.External;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailDTO dto);
}
