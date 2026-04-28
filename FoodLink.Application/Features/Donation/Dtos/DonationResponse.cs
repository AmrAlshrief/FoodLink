namespace FoodLink.Application.Features.Donations.Dtos; 
public class DonationResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset? ExpiryDate { get; set; }
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<DonationItemResponse> Items { get; set; } = new();
}

public class DonationItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public string Unit { get; set; } = string.Empty;
}