using Microsoft.EntityFrameworkCore;
using FoodLink.Domain.Entities;
using FoodLink.Application.Features.Notifications.Interfaces;

namespace FoodLink.Infrastructure.Data.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<List<Notification>> GetUnreadUserNotificationsAsync(Guid userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public void Add(Notification notification)
    {
        context.Notifications.Add(notification);
    }

    public void Update(Notification notification)
    {
        context.Notifications.Update(notification);
    }
}
