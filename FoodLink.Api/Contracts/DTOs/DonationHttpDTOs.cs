using Microsoft.AspNetCore.Http;

namespace FoodLink.Api.Contracts.DTOs;

public record CreateDonationHttpRequest(
    string Title,
    string? Description,
    DateTimeOffset? ExpiryDate,
    IFormFile? Image
);

public record UpdateDonationHttpRequest(
    Guid Id,
    string? Title,
    string? Description,
    DateTimeOffset? ExpiryDate,
    IFormFile? Image
);

public record AddDonationItemHttpRequest(
    string Name,
    int Quantity,
    string Unit,
    IFormFile? Image
);

public record UpdateDonationItemHttpRequest(
    string Name,
    int Quantity,
    string Unit,
    IFormFile? Image
);