using Microsoft.AspNetCore.SignalR;
using FoodLink.Api.Hubs;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Notifications.DTOs;

namespace FoodLink.Api.Services;

public class NotificationHubClient(IHubContext<NotificationHub> hubContext) : INotificationHubClient
{
    public async Task SendNotificationAsync(Guid userId, NotificationResponse notification)
    {
        await hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);
    }
}
