using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}