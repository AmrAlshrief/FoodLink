namespace FoodLink.Application.Features.Charity.Dtos;

public class CharityResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int NoShowCount { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
}
