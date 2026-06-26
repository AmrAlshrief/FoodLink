namespace FoodLink.Application.Features.Reviews.DTOs;

public class CreateReviewRequest
{
    public Guid ReservationId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }
}