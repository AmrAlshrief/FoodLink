namespace FoodLink.Application.Features.Account.Dtos;

public record GetProfileResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string Role,
    string? ProfileImage
);

public record UpdateProfileRequest(
    string? Name,
    string? Phone
);

public record UpdateProfileImageRequest(
    Stream ProfileImage,
    string ProfileImageFileName
);

public record UpdateProfileResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string? ProfileImage,
    string Message
);
