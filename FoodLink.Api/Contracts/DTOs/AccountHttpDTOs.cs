using Microsoft.AspNetCore.Http;

namespace FoodLink.Api.Contracts.DTOs;

public record UpdateProfileHttpRequest(
    string? Name,
    string? Phone
);

public record UpdateProfileImageHttpRequest(
    IFormFile ProfileImage
);
