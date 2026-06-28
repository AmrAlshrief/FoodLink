using FoodLink.Domain.Entities;

namespace FoodLink.Application.Features.Notifications.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
    Task<List<Notification>> GetUnreadUserNotificationsAsync(Guid userId);
    Task<Notification?> GetByIdAsync(Guid id);
    void Add(Notification notification);
    void Update(Notification notification);
}
