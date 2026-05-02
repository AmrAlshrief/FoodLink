namespace FoodLink.Application.Features.Reservation.Dtos;

public class CreateReservationRequest
{
    public Guid CharityId { get; set; }
    public Guid DonationId { get; set; }
    public List<CreateReservationItemRequest> Items { get; set; } = new();
}

public class CreateReservationItemRequest
{
    public Guid DonationItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class ReservationFilterRequest
{
    public Guid? DonationId { get; set; }
    public string? Status { get; set; }
    public bool? IsExpired { get; set; }
}
