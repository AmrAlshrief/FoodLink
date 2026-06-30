using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Notifications.DTOs;
using FoodLink.Application.Features.Notifications.Interfaces;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Entities;

namespace FoodLink.Application.Features.Notifications.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    INotificationHubClient notificationHubClient) : INotificationService
{
    public async Task<List<NotificationResponse>> GetUserNotificationsAsync()
    {
        var userId = userContext.UserId ?? throw new UnauthorizedAccessException();
        var notifications = await notificationRepository.GetUserNotificationsAsync(userId);
        return notifications.Select(MapToResponse).ToList();
    }

    public async Task<List<NotificationResponse>> GetUnreadUserNotificationsAsync()
    {
        var userId = userContext.UserId ?? throw new UnauthorizedAccessException();
        var notifications = await notificationRepository.GetUnreadUserNotificationsAsync(userId);
        return notifications.Select(MapToResponse).ToList();
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        var userId = userContext.UserId ?? throw new UnauthorizedAccessException();
        var notification = await notificationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Notification not found.");

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException("You cannot modify this notification.");

        notification.MarkAsRead();
        notificationRepository.Update(notification);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task CreateNotificationAsync(Guid userId, string title, string message, string targetType, Guid? relatedId = null)
    {
        var notification = new Notification(userId, title, message, targetType, relatedId);
        notificationRepository.Add(notification);
        await unitOfWork.SaveChangesAsync();

        var response = MapToResponse(notification);
        await notificationHubClient.SendNotificationAsync(userId, response);
    }

    private static NotificationResponse MapToResponse(Notification notification)
    {
        return new NotificationResponse
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            TargetType = notification.TargetType,
            RelatedEntityId = notification.RelatedEntityId,
            IsRead = notification.IsRead,
            CreatedAt = DateTime.SpecifyKind(notification.CreatedAtUtc, DateTimeKind.Utc)
        };
    }
}
