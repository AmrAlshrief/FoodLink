namespace FoodLink.Application.Features.Charities.DTOs;

public class CharityPrivateProfileResponse : CharityPublicProfileResponse
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }

    // active reservation count
    public int ActiveReservations { get; set; }
}
