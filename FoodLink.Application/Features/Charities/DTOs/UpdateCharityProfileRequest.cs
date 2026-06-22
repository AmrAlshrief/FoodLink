namespace FoodLink.Application.Features.Charities.DTOs;

public class UpdateCharityProfileRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? OrganizationName { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
}
