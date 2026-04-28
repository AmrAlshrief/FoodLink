namespace FoodLink.Application.Features.Reservation.Dtos;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    public DateTime? PickedUpAt { get; set; }
    //public bool IsExpired => DateTime.UtcNow > ExpiresAt && Status == "Pending";
    public bool IsExpired => DateTime.UtcNow > ExpiresAt && Status == "Pending" || Status == "NoShow";
    public DonationSummaryDto Donation { get; set; } = new();
    public CharitySummaryDto Charity { get; set; } = new();
    public List<ReservationItemResponse> Items { get; set; } = new();

    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }

}

public class DonationSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
}

public class CharitySummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ReservationItemResponse
{
    public Guid DonationItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
}