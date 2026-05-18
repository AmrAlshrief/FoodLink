namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminBusinessResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string BusinessName { get; set; } = default!;

    public string BusinessType { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? ProfileImage { get; set; }

    public bool IsSuspended { get; set; }

    public int TotalDonations { get; set; }

    public int ActiveDonations { get; set; }

    public int ExpiredDonations { get; set; }

    public DateTime CreatedAt { get; set; }
}
