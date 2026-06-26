using System;

namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminDonationResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int TotalItems { get; set; }

    public int ReservedItems { get; set; }

    public DateTime CreatedAt { get; set; }
}
