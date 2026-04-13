namespace FoodLink.Application.Features.Donations.Dtos;

public class UpdateDonationRequest
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset? ExpiryDate { get; set; }

    public Stream? Image { get; set; }

    public string? ImageFileName { get; set; }

    public List<DonationItemDto> Items { get; set; } = new();
}
