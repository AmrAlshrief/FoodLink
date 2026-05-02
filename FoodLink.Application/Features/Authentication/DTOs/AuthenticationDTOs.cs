namespace FoodLink.Application.Features.Authentication.DTOs;

public record RegisterBusinessRequest(
    string Name,
    string Email,
    string Password,
    string Phone,
    Stream? ProfileImage,
    string? ProfileImageFileName,
    string BusinessName,
    string Address,
    string BusinessType
);

public record RegisterCharityRequest(
    string Name,
    string Email,
    string Password,
    string Phone,
    Stream? ProfileImage,
    string? ProfileImageFileName,
    string OrganizationName,
    string LicenseNumber,
    string Address
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
    string? ProfileImage,
    //string? Phone,
    string? OrganizationName,
    //string? Address,
    string Token
);

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    string Phone,
    string? ProfileImage
);
