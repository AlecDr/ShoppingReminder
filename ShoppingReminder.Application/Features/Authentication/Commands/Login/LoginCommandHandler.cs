using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingReminder.Application.Common.Interfaces;
using ShoppingReminder.Application.Common.Models;
using ShoppingReminder.Application.DTOs;

namespace ShoppingReminder.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthenticationResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            return Result.Failure<AuthenticationResponse>("Invalid email or password");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result.Failure<AuthenticationResponse>("Invalid email or password");
        }

        // Check if email is verified (optional - você pode comentar isso se quiser permitir login sem verificação)
        //if (!user.IsEmailVerified)
        //{
        //    return Result.Failure<AuthenticationResponse>("Email not verified. Please check your inbox.");
        //}

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