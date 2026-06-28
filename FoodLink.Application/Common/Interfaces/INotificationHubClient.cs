using FoodLink.Application.Features.Notifications.DTOs;

namespace FoodLink.Application.Common.Interfaces;

public interface INotificationHubClient
{
    Task SendNotificationAsync(Guid userId, NotificationResponse notification);
}
