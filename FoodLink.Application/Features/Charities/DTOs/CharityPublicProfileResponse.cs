namespace FoodLink.Application.Features.Charities.DTOs;

public class CharityPublicProfileResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }

    // stats
    public int TotalReservations { get; set; }
    public int PickedUpCount { get; set; }
    public int NoShowCount { get; set; }
    public int CancelledCount { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
}
