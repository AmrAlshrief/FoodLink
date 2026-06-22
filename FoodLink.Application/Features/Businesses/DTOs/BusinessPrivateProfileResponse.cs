namespace FoodLink.Application.Features.Businesses.DTOs;

public class BusinessPrivateProfileResponse : BusinessPublicProfileResponse
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }

    // extra stats
    public int TotalReservationsReceived { get; set; }
    public int PickedUpCount { get; set; }
    public int NoShowCount { get; set; }
}
