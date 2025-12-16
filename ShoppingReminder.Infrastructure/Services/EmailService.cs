using Microsoft.Extensions.Logging;
using ShoppingReminder.Application.Common.Interfaces;

namespace ShoppingReminder.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailVerificationAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        // TODO: Implement real email sending with MailKit
        _logger.LogInformation("Sending email verification to {Email} with token {Token}", email, token);

        // For now, just log it
        _logger.LogInformation("Verification link: https://yourapp.com/verify-email?token={Token}", token);

        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        // TODO: Implement real email sending with MailKit
        _logger.LogInformation("Sending password reset to {Email} with token {Token}", email, token);

        // For now, just log it
        _logger.LogInformation("Password reset link: https://yourapp.com/reset-password?token={Token}", token);

        return Task.CompletedTask;
    }
}