namespace ShoppingReminder.Application.DTOs;

public record AuthenticationResponse(
    Guid UserId,
    string Email,
    string FullName,
    string Token,
    string RefreshToken,
    DateTime ExpiresAt
);