//using BookIt.Application.DTOs.AuthorizationDTO;
//using BookIt.Application.DTOs.EmailDTO;
//using BookIt.Application.Interfaces.Services;
//using BookIt.Application.Interfaces.Services.External;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using MimeKit;

//namespace BookIt.Persistence.Implementations.Services;

//public class EmailService : IEmailService
//{
//    private readonly IConfiguration _configuration;

//    public EmailService(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public async Task SendEmailAsync(SendEmailDTO sendEmailDTO)
//    {
//        var email = new MimeMessage();
//        email.From.Add(new MailboxAddress(_configuration["MailKitoptions:Mail"], _configuration["MailKitoptions:Mail"]));
//        email.To.Add(MailboxAddress.Parse(sendEmailDTO.ToEmail));
//        email.Subject = sendEmailDTO.Subject;
//        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = sendEmailDTO.Body };

//        using var smtp = new SmtpClient();
//        await smtp.ConnectAsync(_configuration["MailKitoptions:Host"], int.Parse(_configuration["MailKitoptions:Port"]), SecureSocketOptions.StartTls);
//        await smtp.AuthenticateAsync(_configuration["MailKitoptions:Mail"], _configuration["MailKitoptions:Password"]);
//        await smtp.SendAsync(email);
//        await smtp.DisconnectAsync(true);
//    }

//    public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
//    {
//        var emailSendDTO = new SendEmailDTO
//        {
//            ToEmail = email,
//            Subject = "Confirm your email",
//            Body = $@"
//                <h1>Welcome to BookIt!</h1>
//                <p>Please confirm your email by clicking the link below:</p>
//                <p><a href='{confirmationLink}'>Click here to confirm your email</a></p>
//                <p>If you did not request this email, please ignore it.</p>"
//        };

//        await SendEmailAsync(emailSendDTO);
//    }

//    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
//    {
//        var emailSendDTO = new SendEmailDTO
//        {
//            ToEmail = email,
//            Subject = "Reset your password",
//            Body = $@"
//                <h1>Password Reset Request</h1>
//                <p>You have requested to reset your password. Click the link below to proceed:</p>
//                <p><a href='{resetLink}'>Click here to reset your password</a></p>
//                <p>If you did not request this email, please ignore it.</p>"
//        };

//        await SendEmailAsync(emailSendDTO);
//    }
//} 