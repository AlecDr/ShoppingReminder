using MediatR;
using ShoppingReminder.Application.Common.Models;
using ShoppingReminder.Application.DTOs;

namespace ShoppingReminder.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName
) : IRequest<Result<AuthenticationResponse>>;