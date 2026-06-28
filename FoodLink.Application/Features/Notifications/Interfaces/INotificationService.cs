using FoodLink.Application.Features.Notifications.DTOs;

namespace FoodLink.Application.Features.Notifications.Interfaces;

public interface INotificationService
{
    Task<List<NotificationResponse>> GetUserNotificationsAsync();
    Task<List<NotificationResponse>> GetUnreadUserNotificationsAsync();
    Task MarkAsReadAsync(Guid id);
    Task CreateNotificationAsync(Guid userId, string title, string message, string targetType, Guid? relatedId = null);
}
