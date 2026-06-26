
namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminCharityResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = default!;

    public string LicenseNumber { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? ProfileImage { get; set; }

    public bool IsSuspended { get; set; }

    public int TotalReservations { get; set; }

    public int PickedUpReservations { get; set; }

    public int NoShowReservations { get; set; }

    public int CancelledReservations { get; set; }

    public int PendingReservations { get; set; }

    public DateTime CreatedAt { get; set; }
}