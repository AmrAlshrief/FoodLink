namespace FoodLink.Application.Features.Reservation.Dtos;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid DonationId { get; set; }
    public Guid CharityId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? PickedUpAt { get; set; }

    public List<ReservationItemResponse> Items { get; set; } = new();
}

public class ReservationItemResponse
{
    public Guid DonationItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
}