using MediatR;
using ShoppingReminder.Application.Common.Models;
using ShoppingReminder.Application.DTOs;

namespace ShoppingReminder.Application.Features.Authentication.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<AuthenticationResponse>>;