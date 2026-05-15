namespace Mentora.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string toEmail, string firstName, string verificationToken);
    Task SendWelcomeEmailAsync(string toEmail, string firstName, string role);
    Task SendApplicationNotificationAsync(string toEmail, string firstName, string programTitle, string status);
    Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken);
}