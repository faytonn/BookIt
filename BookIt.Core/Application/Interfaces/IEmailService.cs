namespace BookIt.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendVerificationEmailAsync(string to, string verificationToken);
    }
}
