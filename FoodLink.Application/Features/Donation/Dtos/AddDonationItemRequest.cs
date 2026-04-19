namespace FoodLink.Application.Features.Donations.Dtos;

public class AddDonationItemRequest
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
    public string Unit { get; set; } = string.Empty;
}