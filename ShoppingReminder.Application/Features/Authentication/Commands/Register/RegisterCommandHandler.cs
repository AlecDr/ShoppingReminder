using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingReminder.Application.Common.Interfaces;
using ShoppingReminder.Application.Common.Models;
using ShoppingReminder.Application.DTOs;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
    }

    public async Task<Result<AuthenticationResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            return Result.Failure<AuthenticationResponse>("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            FullName = request.FullName,
            IsEmailVerified = false,
            EmailVerificationToken = Guid.NewGuid().ToString(),
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Send verification email (async, don't wait)
        _ = _emailService.SendEmailVerificationAsync(
            user.Email,
            user.EmailVerificationToken!,
            cancellationToken
        );

        // Generate tokens
        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        var response = new AuthenticationResponse(
            user.Id,
            user.Email,
            user.FullName,
            token,
            refreshToken,
            DateTime.UtcNow.AddHours(1)
        );

        return Result.Success(response);
    }
}