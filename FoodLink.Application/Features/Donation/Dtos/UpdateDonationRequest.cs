namespace FoodLink.Application.Features.Donations.Dtos;

public class UpdateDonationRequest
{
    public Guid Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset? ExpiryDate { get; set; }

    public Stream? Image { get; set; }

    public string? ImageFileName { get; set; }

}

public class UpdateDonationItemRequest
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;

    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
}
