namespace FoodLink.Application.Features.Reviews.DTOs;

public class GroupedReviewResponse
{
    public Guid ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public string? ReviewerLogo { get; set; }
    public int ReviewCount { get; set; }
    public List<ReviewResponse> Reviews { get; set; } = new();
}
