using FoodLink.Application.Features.Notifications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserNotifications([FromQuery] bool unreadOnly = false)
    {
        var notifications = unreadOnly 
            ? await notificationService.GetUnreadUserNotificationsAsync()
            : await notificationService.GetUserNotificationsAsync();
            
        return Ok(notifications);
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await notificationService.MarkAsReadAsync(id);
        return NoContent();
    }
}
