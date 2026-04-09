namespace FoodLink.Application.Features.Authentication.DTOs;

public record RegisterRequest(
    string Name,
    string Email,
    string Password,
    string Role,
    string Phone,
    string? ProfileImage
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthenticationResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    string Token
);
