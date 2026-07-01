namespace FoodLink.Application.Features.Businesses.DTOs;

public class BusinessPrivateProfileResponse
{
    // Identity & Basic Info
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    
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

    // Donation Stats
    public int TotalDonations { get; set; }
    public int ActiveDonations { get; set; }
    public int CompletedDonations { get; set; }
    public int CancelledDonations { get; set; }
    public int TotalItemsDonated { get; set; }

    // Reservation Stats
    public int TotalReservationsReceived { get; set; }
    public int PickedUpCount { get; set; }
    public int NoShowCount { get; set; }
}
