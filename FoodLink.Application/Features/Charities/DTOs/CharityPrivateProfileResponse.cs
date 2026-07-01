namespace FoodLink.Application.Features.Charities.DTOs;

public class CharityPrivateProfileResponse
{
    // Identity & Basic Info
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    
    // Contact Info
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Account Status
    public string? ProfileImage { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }

    // Ratings
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }

    // Reservation Stats
    public int TotalReservations { get; set; }
    public int ActiveReservations { get; set; }
    public int PickedUpCount { get; set; }
    public int NoShowCount { get; set; }
    public int CancelledCount { get; set; }
}
