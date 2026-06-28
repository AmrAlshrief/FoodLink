namespace FoodLink.Application.Features.Notifications.DTOs;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public Guid? RelatedEntityId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
