namespace FoodLink.Application.Features.Businesses.DTOs;

public class UpdateBusinessProfileRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? BusinessName { get; set; }
    public string? BusinessType { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
}
