namespace FoodLink.Application.Features.Reviews.DTOs;

public class ReviewResponse
{
    public Guid Id { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public string ReviewerName { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}