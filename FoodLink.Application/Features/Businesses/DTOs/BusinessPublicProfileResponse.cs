namespace FoodLink.Application.Features.Businesses.DTOs;

public class BusinessPublicProfileResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }

    // stats
    public int TotalDonations { get; set; }
    public int ActiveDonations { get; set; }
    public int CompletedDonations { get; set; }
    public int CancelledDonations { get; set; }
    public int TotalItemsDonated { get; set; }
}
