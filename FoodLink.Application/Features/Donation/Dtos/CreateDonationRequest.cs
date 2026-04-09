namespace FoodLink.Application.Features.Donations.Dtos;
public class CreateDonationRequest
{
    public Guid BusinessId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset? ExpiryDate { get; set; }
    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
    public List<DonationItemDto> Items { get; set; } = new();
}

public class DonationItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
    public string Unit { get; set; } = string.Empty;
}