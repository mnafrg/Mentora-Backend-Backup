using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Mentora.Application.Interfaces;
using Mentora.Infrastructure.Configuration;

namespace Mentora.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendVerificationEmailAsync(string toEmail, string firstName, string verificationToken)
    {
        var verificationLink = $"{_emailSettings.FrontendUrl}/verify-email?token={verificationToken}";

        var subject = "Verify Your Email Address";
        var body = $@"
            <html>
            <body>
                <h2>Welcome to Mentorship Platform, {firstName}!</h2>
                <p>Thank you for registering. Please verify your email address by clicking the link below:</p>
                <p><a href='{verificationLink}'>Verify Email Address</a></p>
                <p>This link will expire in 24 hours.</p>
                <p>If you didn't create an account, please ignore this email.</p>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName, string role)
    {
        var subject = "Welcome to Mentorship Platform!";
        var body = $@"
            <html>
            <body>
                <h2>Welcome, {firstName}!</h2>
                <p>Your {role} profile has been created successfully.</p>
                <p>You can now log in and start using the platform.</p>
                <p>If you have any questions, feel free to contact our support team.</p>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }
    public async Task SendApplicationNotificationAsync(string toEmail, string firstName, string programTitle, string status)
    {
        var subject = $"Update on your application for {programTitle}";

        var body = $@"
        <html>
        <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <div style='max-width: 600px; margin: auto; border: 1px solid #eee; padding: 20px;'>
                <h2 style='color: #2c3e50;'>Hello {firstName},</h2>
                <p>We are writing to update you on your mentorship application for <strong>{programTitle}</strong>.</p>
                <div style='padding: 15px; background-color: #f8f9fa; border-left: 5px solid #3498db; margin: 20px 0;'>
                    <strong>Status:</strong> {status}
                </div>
                <p>You can log in to the Mentora platform to see more details about your application.</p>
                <br>
                <p>Best regards,<br><strong>Mentora Team</strong></p>
            </div>
        </body>
        </html>";

        await SendEmailAsync(toEmail, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Log the error (implement proper logging)
            throw new Exception($"Email sending failed: {ex.Message}");
        }
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken)
    {
 
        var frontendUrl = _emailSettings.FrontendUrl;
        var resetLink = $"{frontendUrl}/reset-password?token={resetToken}";

        var subject = "Password Reset Request";
        var body = $@"
        <html>
        <body>
            <h2>Password Reset</h2>
            <p>Hello {userName},</p>
            <p>We received a request to reset your password. Please click the link below to set a new password:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>This link will expire in 1 hour.</p>
            <p>If you did not request a password reset, please ignore this email.</p>
              
            <p>Thank you,</p>
            <p>The Mentora Team</p>
        </body>
        </html>";

        await SendEmailAsync(toEmail, subject, body);
    }
}