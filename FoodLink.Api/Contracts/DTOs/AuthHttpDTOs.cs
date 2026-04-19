using Microsoft.AspNetCore.Http;

namespace FoodLink.Api.Contracts.DTOs;

public record RegisterBusinessHttpRequest(
    string Name,
    string Email,
    string Password,
    string Phone,
    IFormFile? ProfileImage,
    string BusinessName,
    string Address,
    string BusinessType
);

public record RegisterCharityHttpRequest(
    string Name,
    string Email,
    string Password,
    string Phone,
    IFormFile? ProfileImage,
    string OrganizationName,
    string LicenseNumber,
    string Address
);